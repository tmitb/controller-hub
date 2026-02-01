"""Utility script to generate typed C# request wrappers from ``protocol.md``.

The original version only emitted a generic ``JObject`` parameter and return
type.  This updated implementation parses the markdown tables for **Request
Fields** and **Response Fields**, maps them to appropriate C# primitive types,
adds optional parameters (prefixed with ``?`` in the protocol), and produces XML
doc comments that contain the field description – enabling full IntelliSense.

If a response consists of exactly one simple field (String, Number or Boolean)
the generated method returns that primitive type; otherwise it falls back to
``JObject``.
"""

import re
from pathlib import Path

protocol_path = "protocol.md"
base_dir = Path("software/ObsController/Services/Requests")
base_dir.mkdir(parents=True, exist_ok=True)

# ---------------------------------------------------------------------------
# Helper utilities
# ---------------------------------------------------------------------------

TYPE_MAP = {
    "string": "string",
    "number": "double",  # use double for numeric values
    "boolean": "bool",
    "object": "JObject",
    "array<string>": "IReadOnlyList<string>",
    "array<number>": "IReadOnlyList<double>",
    "any": "object",
}

def clean_type(md_type: str) -> str:
    """Normalize markdown type strings to the keys of ``TYPE_MAP``.

    The protocol uses ``Array<String>`` syntax; we lower‑case and replace
    angle‑brackets with a more convenient form.
    """
    t = md_type.strip().lower()
    # Handle generic array notation like ``array<string>``
    if t.startswith("array<"):
        inner = t[6:-1]
        return f"array<{inner}>"
    # Remove markdown escapes
    t = t.replace("&lt;", "<").replace("&gt;", ">")
    # Normalise common forms
    if t.startswith("array<"):
        return t
    return t

def cs_type(md_type: str) -> str:
    """Map a markdown type to a C# type string, defaulting to ``JObject``.
    """
    norm = clean_type(md_type)
    return TYPE_MAP.get(norm, "JObject")

def parse_table(section: str) -> list[dict]:
    """Extract rows from a markdown table after the current heading.

    Returns a list of dictionaries with ``name``, ``type`` and ``desc`` keys.
    """
    lines = []
    in_table = False
    for line in section.splitlines():
        if line.strip().startswith("|"):
            in_table = True
            lines.append(line)
        elif in_table and not line.strip().startswith("|"):
            break
    # No table found
    if len(lines) < 3:
        return []
    rows: list[dict] = []
    for line in lines[2:]:  # skip header and separator
        cols = [c.strip() for c in line.split("|")[1:-1]]
        if len(cols) >= 3:
            rows.append({"name": cols[0], "type": cols[1], "desc": cols[2]})
    return rows

def extract_section(text: str, heading: str) -> str:
    """Return the markdown block that follows ``heading``.

    The protocol uses a horizontal rule (`---`) to separate request definitions.
    We stop at the first occurrence of either a standalone ``---`` line, a new top‑level
    heading (``##``), or the end of the file. Using a non‑greedy match ensures we capture the
    full table even when the separator line itself contains the string ``---`` as part of markdown.
    """
    # Match until a blank line with three hyphens, a new top-level heading, or EOF.
    # ``---`` that appears as a table separator is part of the row and should not
    # terminate the capture. We therefore look for a line that consists solely of three hyphens.
    pattern = re.compile(
        re.escape(heading) + r"(.*?)(?:\n---\s*$|^##\s|\Z)",
        re.DOTALL | re.MULTILINE,
    )
    m = pattern.search(text)
    if not m:
        return ""
    return m.group(1).strip()

# ---------------------------------------------------------------------------
# Parse the protocol document
# ---------------------------------------------------------------------------

with open(protocol_path, "r", encoding="utf-8") as f:
    text = f.read()

# Find category sections (e.g., "## General Requests") using a line‑anchored regex so that
# sub‑headings like ``###`` are not mistaken for a new category.
category_regex = re.compile(r"(?m)^##\s+(.*?)\s+Requests", re.IGNORECASE)
categories: list[tuple[str, str]] = []
for m in category_regex.finditer(text):
    cat_name = m.group(1).strip()
    start = m.end()
    # Look ahead for the next top‑level ``##`` heading (anchored at line start)
    next_match = re.search(r"(?m)^##\s+", text[start:])
    end = start + (next_match.start() if next_match else len(text))
    cat_body = text[start:end]
    categories.append((cat_name, cat_body))

# ---------------------------------------------------------------------------
# Generate C# wrapper classes
# ---------------------------------------------------------------------------

for cat_name, cat_body in categories:
    class_name = "".join(w.title() for w in cat_name.split()) + "Requests"
    file_path = base_dir / f"{class_name}.cs"
    lines: list[str] = []
    lines.append("using System.Threading.Tasks;")
    lines.append("using Newtonsoft.Json.Linq;")
    lines.append("")
    lines.append("namespace ObsController.Services.Requests;")
    # Inherit from BaseRequests and delegate bridge initialization to base class
    lines.append(f"public class {class_name} : BaseRequests")
    lines.append("{")
    lines.append(f"    public {class_name}(ObsBridge bridge) : base(bridge) {{}}")

    # Iterate over each request definition within the category
    for req_match in re.finditer(r"###\s+(\w+)", cat_body):
        req_name = req_match.group(1)
        # Extract the markdown block belonging to this request (up to next heading or --- separator)
        start_idx = req_match.end()
        sub_text = cat_body[start_idx:]

        end_marker = re.search(r"^###\s", sub_text, re.MULTILINE)
        if end_marker:
            req_section = sub_text[: end_marker.start()]
        else:
            req_section = sub_text

        # Parse request fields and response fields tables
        request_table_md = extract_section(req_section, "**Request Fields:**")
        request_fields = parse_table(request_table_md)
        response_table_md = extract_section(req_section, "**Response Fields:**")
        response_fields = parse_table(response_table_md)

        # -------------------------------------------------------------------
        # Build method signature and documentation based on request fields.
        # -------------------------------------------------------------------
        # Separate required and optional parameters to ensure correct ordering.
        required_parts: list[str] = []
        optional_parts: list[str] = []
        doc_params: list[str] = []
        for field in request_fields:
            raw_name = field["name"]
            # Skip nested fields like "keyModifiers.shift"
            if "." in raw_name:
                continue
            optional = raw_name.startswith("?")
            name = raw_name.lstrip("?")
            cs_typ = cs_type(field["type"])
            # Determine nullable for value types when optional
            if optional and cs_typ in {"double", "bool", "int", "long", "float", "decimal"}:
                cs_typ += "?"
            # Prefer concrete types; fall back to JObject for complex structures.
            if cs_typ == "JObject":
                param_decl = f"JObject {name}"
            else:
                param_decl = f"{cs_typ} {name}"
            if optional:
                param_decl += " = null"
                optional_parts.append(param_decl)
            else:
                required_parts.append(param_decl)
            doc_params.append(f"/// <param name=\"{name}\">{field['desc']}</param>")
        # Concatenate required then optional parameters.
        param_parts = required_parts + optional_parts

        params_str = ", ".join(param_parts) if param_parts else ""

        # -------------------------------------------------------------------
        # Determine return type from response fields.
        # If there is exactly one simple field, use its concrete C# type; otherwise
        # default to JObject for complex or multiple values.
        # -------------------------------------------------------------------
        if len(response_fields) == 1:
            cs_ret = cs_type(response_fields[0]["type"])
            if cs_ret not in {"string", "double", "bool", "int", "long", "JObject"}:
                cs_ret = "JObject"
        else:
            cs_ret = "JObject"

        # -------------------------------------------------------------------
        # Extract the description text that appears directly under the request
        # heading, up to the first occurrence of either a Request or Response
        # fields table. The markdown uses bold markers like "**Request Fields:**"
        # and "**Response Fields:**" – we locate whichever comes first.
        # -------------------------------------------------------------------
        def _extract_summary(text: str) -> str:
            """Extract the description block preceding any tables.

            The markdown format places a free‑form description directly after the
            ``### <Name>`` heading, followed by optional metadata lines (starting
            with ``--``) and then the ``**Request Fields:**`` / ``**Response
            Fields:**`` tables. For IntelliSense we want exactly the description
            text up to the first of those tables, ignoring any leading/trailing
            blank lines.
            """
            # Find earliest occurrence of a table marker.
            lower = text.lower()
            req_idx = lower.find("**request fields:**")
            resp_idx = lower.find("**response fields:**")
            cut_idx = -1
            if req_idx != -1 and resp_idx != -1:
                cut_idx = min(req_idx, resp_idx)
            elif req_idx != -1:
                cut_idx = req_idx
            elif resp_idx != -1:
                cut_idx = resp_idx

            # Slice up to the marker if found; otherwise use whole text.
            slice_text = text[:cut_idx] if cut_idx != -1 else text
            # Return the raw description and metadata block (before tables)
            # collapsing newlines into spaces to form a single‑line summary.
            return " ".join(slice_text.split()).strip()

        summary_text = _extract_summary(req_section)

        # -------------------------------------------------------------------
        # Emit XML docs and method stub.
        # -------------------------------------------------------------------
        lines.append(f"    /// <summary>{summary_text}</summary>")
        for doc in doc_params:
            lines.append(f"    {doc}")
        # Emit method signature, handling primitive return types.
        if cs_ret == "JObject":
            lines.append(f"    public Task<JObject> {req_name}Async({params_str})")
        else:
            lines.append(f"    public async Task<{cs_ret}> {req_name}Async({params_str})")

        # Build request payload. If there are parameters, construct a JObject and add
        # each non‑null argument; otherwise send an empty object.
        if param_parts:
            lines.append("    {")
            lines.append("        var data = new JObject();")
            for field in request_fields:
                raw_name = field["name"]
                optional = raw_name.startswith("?")
                name = raw_name.lstrip("?")
                if optional:
                    lines.append(f"        if ({name} != null) data[\"{name}\"] = JToken.FromObject({name});")
                else:
                    lines.append(f"        data[\"{name}\"] = JToken.FromObject({name});")

            # Call bridge and extract response if needed.
            if cs_ret == "JObject":
                lines.append(f"        return _bridge.SendRequestAsync(\"{req_name}\", data);")
            else:
                resp_field = response_fields[0]["name"]
                lines.append(f"        var resp = await _bridge.SendRequestAsync(\"{req_name}\", data);")
                lines.append(f"        return resp[\"{resp_field}\"].ToObject<{cs_ret}>();")
            lines.append("    }")
        else:
            if cs_ret == "JObject":
                lines.append(f"    => _bridge.SendRequestAsync(\"{req_name}\", new JObject());")
            else:
                resp_field = response_fields[0]["name"] if response_fields else ""
                lines.append("    {")
                lines.append(f"        var resp = await _bridge.SendRequestAsync(\"{req_name}\", new JObject());")
                lines.append(f"        return resp[\"{resp_field}\"].ToObject<{cs_ret}>();")
                lines.append("    }")

    lines.append("}")

    # Write the generated file
    with open(file_path, "w", encoding="utf-8") as f:
        f.write("\n".join(lines))
