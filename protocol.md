# Requests

## General Requests

### GetVersion

Gets data about the current plugin and RPC version.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| obsVersion | String | Current OBS Studio version |
| obsWebSocketVersion | String | Current obs-websocket version |
| rpcVersion | Number | Current latest obs-websocket RPC version |
| availableRequests | Array&lt;String&gt; | Array of available RPC requests for the currently negotiated RPC version |
| supportedImageFormats | Array&lt;String&gt; | Image formats available in `GetSourceScreenshot` and `SaveSourceScreenshot` requests. |
| platform | String | Name of the platform. Usually `windows`, `macos`, or `ubuntu` (linux flavor). Not guaranteed to be any of those |
| platformDescription | String | Description of the platform, like `Windows 10 (10.0)` |

---

### GetStats

Gets statistics about OBS, obs-websocket, and the current session.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| cpuUsage | Number | Current CPU usage in percent |
| memoryUsage | Number | Amount of memory in MB currently being used by OBS |
| availableDiskSpace | Number | Available disk space on the device being used for recording storage |
| activeFps | Number | Current FPS being rendered |
| averageFrameRenderTime | Number | Average time in milliseconds that OBS is taking to render a frame |
| renderSkippedFrames | Number | Number of frames skipped by OBS in the render thread |
| renderTotalFrames | Number | Total number of frames outputted by the render thread |
| outputSkippedFrames | Number | Number of frames skipped by OBS in the output thread |
| outputTotalFrames | Number | Total number of frames outputted by the output thread |
| webSocketSessionIncomingMessages | Number | Total number of messages received by obs-websocket from the client |
| webSocketSessionOutgoingMessages | Number | Total number of messages sent by obs-websocket to the client |

---

### BroadcastCustomEvent

Broadcasts a `CustomEvent` to all WebSocket clients. Receivers are clients which are identified and subscribed.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| eventData | Object | Data payload to emit to all receivers | None | N/A |

---

### CallVendorRequest

Call a request registered to a vendor.

A vendor is a unique name registered by a third-party plugin or script, which allows for custom requests and events to be added to obs-websocket.
If a plugin or script implements vendor requests or events, documentation is expected to be provided with them.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| vendorName | String | Name of the vendor to use | None | N/A |
| requestType | String | The request type to call | None | N/A |
| ?requestData | Object | Object containing appropriate request data | None | {} |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| vendorName | String | Echoed of `vendorName` |
| requestType | String | Echoed of `requestType` |
| responseData | Object | Object containing appropriate response data. {} if request does not provide any response data |

---

### GetHotkeyList

Gets an array of all hotkey names in OBS.

Note: Hotkey functionality in obs-websocket comes as-is, and we do not guarantee support if things are broken. In 9/10 usages of hotkey requests, there exists a better, more reliable method via other requests.

- Complexity Rating: `4/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| hotkeys | Array&lt;String&gt; | Array of hotkey names |

---

### TriggerHotkeyByName

Triggers a hotkey using its name. See `GetHotkeyList`.

Note: Hotkey functionality in obs-websocket comes as-is, and we do not guarantee support if things are broken. In 9/10 usages of hotkey requests, there exists a better, more reliable method via other requests.

- Complexity Rating: `4/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| hotkeyName | String | Name of the hotkey to trigger | None | N/A |
| ?contextName | String | Name of context of the hotkey to trigger | None | Unknown |

---

### TriggerHotkeyByKeySequence

Triggers a hotkey using a sequence of keys.

Note: Hotkey functionality in obs-websocket comes as-is, and we do not guarantee support if things are broken. In 9/10 usages of hotkey requests, there exists a better, more reliable method via other requests.

- Complexity Rating: `4/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?keyId | String | The OBS key ID to use. See https://github.com/obsproject/obs-studio/blob/master/libobs/obs-hotkeys.h | None | Not pressed |
| ?keyModifiers | Object | Object containing key modifiers to apply | None | Ignored |
| ?keyModifiers.shift | Boolean | Press Shift | None | Not pressed |
| ?keyModifiers.control | Boolean | Press CTRL | None | Not pressed |
| ?keyModifiers.alt | Boolean | Press ALT | None | Not pressed |
| ?keyModifiers.command | Boolean | Press CMD (Mac) | None | Not pressed |

---

### Sleep

Sleeps for a time duration or number of frames. Only available in request batches with types `SERIAL_REALTIME` or `SERIAL_FRAME`.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sleepMillis | Number | Number of milliseconds to sleep for (if `SERIAL_REALTIME` mode) | >= 0, <= 50000 | Unknown |
| ?sleepFrames | Number | Number of frames to sleep for (if `SERIAL_FRAME` mode) | >= 0, <= 10000 | Unknown |

## Config Requests

### GetPersistentData

Gets the value of a "slot" from the selected persistent data realm.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| realm | String | The data realm to select. `OBS_WEBSOCKET_DATA_REALM_GLOBAL` or `OBS_WEBSOCKET_DATA_REALM_PROFILE` | None | N/A |
| slotName | String | The name of the slot to retrieve data from | None | N/A |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| slotValue | Any | Value associated with the slot. `null` if not set |

---

### SetPersistentData

Sets the value of a "slot" from the selected persistent data realm.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| realm | String | The data realm to select. `OBS_WEBSOCKET_DATA_REALM_GLOBAL` or `OBS_WEBSOCKET_DATA_REALM_PROFILE` | None | N/A |
| slotName | String | The name of the slot to retrieve data from | None | N/A |
| slotValue | Any | The value to apply to the slot | None | N/A |

---

### GetSceneCollectionList

Gets an array of all scene collections

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| currentSceneCollectionName | String | The name of the current scene collection |
| sceneCollections | Array&lt;String&gt; | Array of all available scene collections |

---

### SetCurrentSceneCollection

Switches to a scene collection.

Note: This will block until the collection has finished changing.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| sceneCollectionName | String | Name of the scene collection to switch to | None | N/A |

---

### CreateSceneCollection

Creates a new scene collection, switching to it in the process.

Note: This will block until the collection has finished changing.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| sceneCollectionName | String | Name for the new scene collection | None | N/A |

---

### GetProfileList

Gets an array of all profiles

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| currentProfileName | String | The name of the current profile |
| profiles | Array&lt;String&gt; | Array of all available profiles |

---

### SetCurrentProfile

Switches to a profile.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| profileName | String | Name of the profile to switch to | None | N/A |

---

### CreateProfile

Creates a new profile, switching to it in the process

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| profileName | String | Name for the new profile | None | N/A |

---

### RemoveProfile

Removes a profile. If the current profile is chosen, it will change to a different profile first.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| profileName | String | Name of the profile to remove | None | N/A |

---

### GetProfileParameter

Gets a parameter from the current profile's configuration.

- Complexity Rating: `4/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| parameterCategory | String | Category of the parameter to get | None | N/A |
| parameterName | String | Name of the parameter to get | None | N/A |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| parameterValue | String | Value associated with the parameter. `null` if not set and no default |
| defaultParameterValue | String | Default value associated with the parameter. `null` if no default |

---

### SetProfileParameter

Sets the value of a parameter in the current profile's configuration.

- Complexity Rating: `4/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| parameterCategory | String | Category of the parameter to set | None | N/A |
| parameterName | String | Name of the parameter to set | None | N/A |
| parameterValue | String | Value of the parameter to set. Use `null` to delete | None | N/A |

---

### GetVideoSettings

Gets the current video settings.

Note: To get the true FPS value, divide the FPS numerator by the FPS denominator. Example: `60000/1001`

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| fpsNumerator | Number | Numerator of the fractional FPS value |
| fpsDenominator | Number | Denominator of the fractional FPS value |
| baseWidth | Number | Width of the base (canvas) resolution in pixels |
| baseHeight | Number | Height of the base (canvas) resolution in pixels |
| outputWidth | Number | Width of the output resolution in pixels |
| outputHeight | Number | Height of the output resolution in pixels |

---

### SetVideoSettings

Sets the current video settings.

Note: Fields must be specified in pairs. For example, you cannot set only `baseWidth` without needing to specify `baseHeight`.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?fpsNumerator | Number | Numerator of the fractional FPS value | >= 1 | Not changed |
| ?fpsDenominator | Number | Denominator of the fractional FPS value | >= 1 | Not changed |
| ?baseWidth | Number | Width of the base (canvas) resolution in pixels | >= 1, <= 4096 | Not changed |
| ?baseHeight | Number | Height of the base (canvas) resolution in pixels | >= 1, <= 4096 | Not changed |
| ?outputWidth | Number | Width of the output resolution in pixels | >= 1, <= 4096 | Not changed |
| ?outputHeight | Number | Height of the output resolution in pixels | >= 1, <= 4096 | Not changed |

---

### GetStreamServiceSettings

Gets the current stream service settings (stream destination).

- Complexity Rating: `4/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| streamServiceType | String | Stream service type, like `rtmp_custom` or `rtmp_common` |
| streamServiceSettings | Object | Stream service settings |

---

### SetStreamServiceSettings

Sets the current stream service settings (stream destination).

Note: Simple RTMP settings can be set with type `rtmp_custom` and the settings fields `server` and `key`.

- Complexity Rating: `4/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| streamServiceType | String | Type of stream service to apply. Example: `rtmp_common` or `rtmp_custom` | None | N/A |
| streamServiceSettings | Object | Settings to apply to the service | None | N/A |

---

### GetRecordDirectory

Gets the current directory that the record output is set to.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| recordDirectory | String | Output directory |

---

### SetRecordDirectory

Sets the current directory that the record output writes files to.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.3.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| recordDirectory | String | Output directory | None | N/A |

## Sources Requests

### GetSourceActive

Gets the active and show state of a source.

**Compatible with inputs and scenes.**

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sourceName | String | Name of the source to get the active state of | None | Unknown |
| ?sourceUuid | String | UUID of the source to get the active state of | None | Unknown |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| videoActive | Boolean | Whether the source is showing in Program |
| videoShowing | Boolean | Whether the source is showing in the UI (Preview, Projector, Properties) |

---

### GetSourceScreenshot

Gets a Base64-encoded screenshot of a source.

The `imageWidth` and `imageHeight` parameters are treated as "scale to inner", meaning the smallest ratio will be used and the aspect ratio of the original resolution is kept.
If `imageWidth` and `imageHeight` are not specified, the compressed image will use the full resolution of the source.

**Compatible with inputs and scenes.**

- Complexity Rating: `4/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sourceName | String | Name of the source to take a screenshot of | None | Unknown |
| ?sourceUuid | String | UUID of the source to take a screenshot of | None | Unknown |
| imageFormat | String | Image compression format to use. Use `GetVersion` to get compatible image formats | None | N/A |
| ?imageWidth | Number | Width to scale the screenshot to | >= 8, <= 4096 | Source value is used |
| ?imageHeight | Number | Height to scale the screenshot to | >= 8, <= 4096 | Source value is used |
| ?imageCompressionQuality | Number | Compression quality to use. 0 for high compression, 100 for uncompressed. -1 to use "default" (whatever that means, idk) | >= -1, <= 100 | -1 |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| imageData | String | Base64-encoded screenshot |

---

### SaveSourceScreenshot

Saves a screenshot of a source to the filesystem.

The `imageWidth` and `imageHeight` parameters are treated as "scale to inner", meaning the smallest ratio will be used and the aspect ratio of the original resolution is kept.
If `imageWidth` and `imageHeight` are not specified, the compressed image will use the full resolution of the source.

**Compatible with inputs and scenes.**

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sourceName | String | Name of the source to take a screenshot of | None | Unknown |
| ?sourceUuid | String | UUID of the source to take a screenshot of | None | Unknown |
| imageFormat | String | Image compression format to use. Use `GetVersion` to get compatible image formats | None | N/A |
| imageFilePath | String | Path to save the screenshot file to. Eg. `C:\Users\user\Desktop\screenshot.png` | None | N/A |
| ?imageWidth | Number | Width to scale the screenshot to | >= 8, <= 4096 | Source value is used |
| ?imageHeight | Number | Height to scale the screenshot to | >= 8, <= 4096 | Source value is used |
| ?imageCompressionQuality | Number | Compression quality to use. 0 for high compression, 100 for uncompressed. -1 to use "default" (whatever that means, idk) | >= -1, <= 100 | -1 |

## Scenes Requests

### GetSceneList

Gets an array of all scenes in OBS.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| currentProgramSceneName | String | Current program scene name. Can be `null` if internal state desync |
| currentProgramSceneUuid | String | Current program scene UUID. Can be `null` if internal state desync |
| currentPreviewSceneName | String | Current preview scene name. `null` if not in studio mode |
| currentPreviewSceneUuid | String | Current preview scene UUID. `null` if not in studio mode |
| scenes | Array&lt;Object&gt; | Array of scenes |

---

### GetGroupList

Gets an array of all groups in OBS.

Groups in OBS are actually scenes, but renamed and modified. In obs-websocket, we treat them as scenes where we can.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| groups | Array&lt;String&gt; | Array of group names |

---

### GetCurrentProgramScene

Gets the current program scene.

Note: This request is slated to have the `currentProgram`-prefixed fields removed from in an upcoming RPC version.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| sceneName | String | Current program scene name |
| sceneUuid | String | Current program scene UUID |
| currentProgramSceneName | String | Current program scene name (Deprecated) |
| currentProgramSceneUuid | String | Current program scene UUID (Deprecated) |

---

### SetCurrentProgramScene

Sets the current program scene.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Scene name to set as the current program scene | None | Unknown |
| ?sceneUuid | String | Scene UUID to set as the current program scene | None | Unknown |

---

### GetCurrentPreviewScene

Gets the current preview scene.

Only available when studio mode is enabled.

Note: This request is slated to have the `currentPreview`-prefixed fields removed from in an upcoming RPC version.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| sceneName | String | Current preview scene name |
| sceneUuid | String | Current preview scene UUID |
| currentPreviewSceneName | String | Current preview scene name |
| currentPreviewSceneUuid | String | Current preview scene UUID |

---

### SetCurrentPreviewScene

Sets the current preview scene.

Only available when studio mode is enabled.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Scene name to set as the current preview scene | None | Unknown |
| ?sceneUuid | String | Scene UUID to set as the current preview scene | None | Unknown |

---

### CreateScene

Creates a new scene in OBS.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| sceneName | String | Name for the new scene | None | N/A |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| sceneUuid | String | UUID of the created scene |

---

### RemoveScene

Removes a scene from OBS.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene to remove | None | Unknown |
| ?sceneUuid | String | UUID of the scene to remove | None | Unknown |

---

### SetSceneName

Sets the name of a scene (rename).

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene to be renamed | None | Unknown |
| ?sceneUuid | String | UUID of the scene to be renamed | None | Unknown |
| newSceneName | String | New name for the scene | None | N/A |

---

### GetSceneSceneTransitionOverride

Gets the scene transition overridden for a scene.

Note: A transition UUID response field is not currently able to be implemented as of 2024-1-18.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene | None | Unknown |
| ?sceneUuid | String | UUID of the scene | None | Unknown |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| transitionName | String | Name of the overridden scene transition, else `null` |
| transitionDuration | Number | Duration of the overridden scene transition, else `null` |

---

### SetSceneSceneTransitionOverride

Sets the scene transition overridden for a scene.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene | None | Unknown |
| ?sceneUuid | String | UUID of the scene | None | Unknown |
| ?transitionName | String | Name of the scene transition to use as override. Specify `null` to remove | None | Unchanged |
| ?transitionDuration | Number | Duration to use for any overridden transition. Specify `null` to remove | >= 50, <= 20000 | Unchanged |

## Inputs Requests

### GetInputList

Gets an array of all inputs in OBS.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputKind | String | Restrict the array to only inputs of the specified kind | None | All kinds included |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| inputs | Array&lt;Object&gt; | Array of inputs |

---

### GetInputKindList

Gets an array of all available input kinds in OBS.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?unversioned | Boolean | True == Return all kinds as unversioned, False == Return with version suffixes (if available) | None | false |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| inputKinds | Array&lt;String&gt; | Array of input kinds |

---

### GetSpecialInputs

Gets the names of all special inputs.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| desktop1 | String | Name of the Desktop Audio input |
| desktop2 | String | Name of the Desktop Audio 2 input |
| mic1 | String | Name of the Mic/Auxiliary Audio input |
| mic2 | String | Name of the Mic/Auxiliary Audio 2 input |
| mic3 | String | Name of the Mic/Auxiliary Audio 3 input |
| mic4 | String | Name of the Mic/Auxiliary Audio 4 input |

---

### CreateInput

Creates a new input, adding it as a scene item to the specified scene.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene to add the input to as a scene item | None | Unknown |
| ?sceneUuid | String | UUID of the scene to add the input to as a scene item | None | Unknown |
| inputName | String | Name of the new input to created | None | N/A |
| inputKind | String | The kind of input to be created | None | N/A |
| ?inputSettings | Object | Settings object to initialize the input with | None | Default settings used |
| ?sceneItemEnabled | Boolean | Whether to set the created scene item to enabled or disabled | None | True |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| inputUuid | String | UUID of the newly created input |
| sceneItemId | Number | ID of the newly created scene item |

---

### RemoveInput

Removes an existing input.

Note: Will immediately remove all associated scene items.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input to remove | None | Unknown |
| ?inputUuid | String | UUID of the input to remove | None | Unknown |

---

### SetInputName

Sets the name of an input (rename).

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Current input name | None | Unknown |
| ?inputUuid | String | Current input UUID | None | Unknown |
| newInputName | String | New name for the input | None | N/A |

---

### GetInputDefaultSettings

Gets the default settings for an input kind.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| inputKind | String | Input kind to get the default settings for | None | N/A |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| defaultInputSettings | Object | Object of default settings for the input kind |

---

### GetInputSettings

Gets the settings of an input.

Note: Does not include defaults. To create the entire settings object, overlay `inputSettings` over the `defaultInputSettings` provided by `GetInputDefaultSettings`.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input to get the settings of | None | Unknown |
| ?inputUuid | String | UUID of the input to get the settings of | None | Unknown |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| inputSettings | Object | Object of settings for the input |
| inputKind | String | The kind of the input |

---

### SetInputSettings

Sets the settings of an input.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input to set the settings of | None | Unknown |
| ?inputUuid | String | UUID of the input to set the settings of | None | Unknown |
| inputSettings | Object | Object of settings to apply | None | N/A |
| ?overlay | Boolean | True == apply the settings on top of existing ones, False == reset the input to its defaults, then apply settings. | None | true |

---

### GetInputMute

Gets the audio mute state of an input.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of input to get the mute state of | None | Unknown |
| ?inputUuid | String | UUID of input to get the mute state of | None | Unknown |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| inputMuted | Boolean | Whether the input is muted |

---

### SetInputMute

Sets the audio mute state of an input.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input to set the mute state of | None | Unknown |
| ?inputUuid | String | UUID of the input to set the mute state of | None | Unknown |
| inputMuted | Boolean | Whether to mute the input or not | None | N/A |

---

### ToggleInputMute

Toggles the audio mute state of an input.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input to toggle the mute state of | None | Unknown |
| ?inputUuid | String | UUID of the input to toggle the mute state of | None | Unknown |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| inputMuted | Boolean | Whether the input has been muted or unmuted |

---

### GetInputVolume

Gets the current volume setting of an input.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input to get the volume of | None | Unknown |
| ?inputUuid | String | UUID of the input to get the volume of | None | Unknown |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| inputVolumeMul | Number | Volume setting in mul |
| inputVolumeDb | Number | Volume setting in dB |

---

### SetInputVolume

Sets the volume setting of an input.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input to set the volume of | None | Unknown |
| ?inputUuid | String | UUID of the input to set the volume of | None | Unknown |
| ?inputVolumeMul | Number | Volume setting in mul | >= 0, <= 20 | `inputVolumeDb` should be specified |
| ?inputVolumeDb | Number | Volume setting in dB | >= -100, <= 26 | `inputVolumeMul` should be specified |

---

### GetInputAudioBalance

Gets the audio balance of an input.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input to get the audio balance of | None | Unknown |
| ?inputUuid | String | UUID of the input to get the audio balance of | None | Unknown |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| inputAudioBalance | Number | Audio balance value from 0.0-1.0 |

---

### SetInputAudioBalance

Sets the audio balance of an input.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input to set the audio balance of | None | Unknown |
| ?inputUuid | String | UUID of the input to set the audio balance of | None | Unknown |
| inputAudioBalance | Number | New audio balance value | >= 0.0, <= 1.0 | N/A |

---

### GetInputAudioSyncOffset

Gets the audio sync offset of an input.

Note: The audio sync offset can be negative too!

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input to get the audio sync offset of | None | Unknown |
| ?inputUuid | String | UUID of the input to get the audio sync offset of | None | Unknown |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| inputAudioSyncOffset | Number | Audio sync offset in milliseconds |

---

### SetInputAudioSyncOffset

Sets the audio sync offset of an input.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input to set the audio sync offset of | None | Unknown |
| ?inputUuid | String | UUID of the input to set the audio sync offset of | None | Unknown |
| inputAudioSyncOffset | Number | New audio sync offset in milliseconds | >= -950, <= 20000 | N/A |

---

### GetInputAudioMonitorType

Gets the audio monitor type of an input.

The available audio monitor types are:

- `OBS_MONITORING_TYPE_NONE`
- `OBS_MONITORING_TYPE_MONITOR_ONLY`
- `OBS_MONITORING_TYPE_MONITOR_AND_OUTPUT`

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input to get the audio monitor type of | None | Unknown |
| ?inputUuid | String | UUID of the input to get the audio monitor type of | None | Unknown |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| monitorType | String | Audio monitor type |

---

### SetInputAudioMonitorType

Sets the audio monitor type of an input.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input to set the audio monitor type of | None | Unknown |
| ?inputUuid | String | UUID of the input to set the audio monitor type of | None | Unknown |
| monitorType | String | Audio monitor type | None | N/A |

---

### GetInputAudioTracks

Gets the enable state of all audio tracks of an input.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input | None | Unknown |
| ?inputUuid | String | UUID of the input | None | Unknown |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| inputAudioTracks | Object | Object of audio tracks and associated enable states |

---

### SetInputAudioTracks

Sets the enable state of audio tracks of an input.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input | None | Unknown |
| ?inputUuid | String | UUID of the input | None | Unknown |
| inputAudioTracks | Object | Track settings to apply | None | N/A |

---

### GetInputDeinterlaceMode

Gets the deinterlace mode of an input.

Deinterlace Modes:

- `OBS_DEINTERLACE_MODE_DISABLE`
- `OBS_DEINTERLACE_MODE_DISCARD`
- `OBS_DEINTERLACE_MODE_RETRO`
- `OBS_DEINTERLACE_MODE_BLEND`
- `OBS_DEINTERLACE_MODE_BLEND_2X`
- `OBS_DEINTERLACE_MODE_LINEAR`
- `OBS_DEINTERLACE_MODE_LINEAR_2X`
- `OBS_DEINTERLACE_MODE_YADIF`
- `OBS_DEINTERLACE_MODE_YADIF_2X`

Note: Deinterlacing functionality is restricted to async inputs only.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.6.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input | None | Unknown |
| ?inputUuid | String | UUID of the input | None | Unknown |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| inputDeinterlaceMode | String | Deinterlace mode of the input |

---

### SetInputDeinterlaceMode

Sets the deinterlace mode of an input.

Note: Deinterlacing functionality is restricted to async inputs only.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.6.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input | None | Unknown |
| ?inputUuid | String | UUID of the input | None | Unknown |
| inputDeinterlaceMode | String | Deinterlace mode for the input | None | N/A |

---

### GetInputDeinterlaceFieldOrder

Gets the deinterlace field order of an input.

Deinterlace Field Orders:

- `OBS_DEINTERLACE_FIELD_ORDER_TOP`
- `OBS_DEINTERLACE_FIELD_ORDER_BOTTOM`

Note: Deinterlacing functionality is restricted to async inputs only.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.6.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input | None | Unknown |
| ?inputUuid | String | UUID of the input | None | Unknown |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| inputDeinterlaceFieldOrder | String | Deinterlace field order of the input |

---

### SetInputDeinterlaceFieldOrder

Sets the deinterlace field order of an input.

Note: Deinterlacing functionality is restricted to async inputs only.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.6.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input | None | Unknown |
| ?inputUuid | String | UUID of the input | None | Unknown |
| inputDeinterlaceFieldOrder | String | Deinterlace field order for the input | None | N/A |

---

### GetInputPropertiesListPropertyItems

Gets the items of a list property from an input's properties.

Note: Use this in cases where an input provides a dynamic, selectable list of items. For example, display capture, where it provides a list of available displays.

- Complexity Rating: `4/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input | None | Unknown |
| ?inputUuid | String | UUID of the input | None | Unknown |
| propertyName | String | Name of the list property to get the items of | None | N/A |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| propertyItems | Array&lt;Object&gt; | Array of items in the list property |

---

### PressInputPropertiesButton

Presses a button in the properties of an input.

Some known `propertyName` values are:

- `refreshnocache` - Browser source reload button

Note: Use this in cases where there is a button in the properties of an input that cannot be accessed in any other way. For example, browser sources, where there is a refresh button.

- Complexity Rating: `4/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input | None | Unknown |
| ?inputUuid | String | UUID of the input | None | Unknown |
| propertyName | String | Name of the button property to press | None | N/A |

## Transitions Requests

### GetTransitionKindList

Gets an array of all available transition kinds.

Similar to `GetInputKindList`

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| transitionKinds | Array&lt;String&gt; | Array of transition kinds |

---

### GetSceneTransitionList

Gets an array of all scene transitions in OBS.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| currentSceneTransitionName | String | Name of the current scene transition. Can be null |
| currentSceneTransitionUuid | String | UUID of the current scene transition. Can be null |
| currentSceneTransitionKind | String | Kind of the current scene transition. Can be null |
| transitions | Array&lt;Object&gt; | Array of transitions |

---

### GetCurrentSceneTransition

Gets information about the current scene transition.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| transitionName | String | Name of the transition |
| transitionUuid | String | UUID of the transition |
| transitionKind | String | Kind of the transition |
| transitionFixed | Boolean | Whether the transition uses a fixed (unconfigurable) duration |
| transitionDuration | Number | Configured transition duration in milliseconds. `null` if transition is fixed |
| transitionConfigurable | Boolean | Whether the transition supports being configured |
| transitionSettings | Object | Object of settings for the transition. `null` if transition is not configurable |

---

### SetCurrentSceneTransition

Sets the current scene transition.

Small note: While the namespace of scene transitions is generally unique, that uniqueness is not a guarantee as it is with other resources like inputs.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| transitionName | String | Name of the transition to make active | None | N/A |

---

### SetCurrentSceneTransitionDuration

Sets the duration of the current scene transition, if it is not fixed.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| transitionDuration | Number | Duration in milliseconds | >= 50, <= 20000 | N/A |

---

### SetCurrentSceneTransitionSettings

Sets the settings of the current scene transition.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| transitionSettings | Object | Settings object to apply to the transition. Can be `{}` | None | N/A |
| ?overlay | Boolean | Whether to overlay over the current settings or replace them | None | true |

---

### GetCurrentSceneTransitionCursor

Gets the cursor position of the current scene transition.

Note: `transitionCursor` will return 1.0 when the transition is inactive.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| transitionCursor | Number | Cursor position, between 0.0 and 1.0 |

---

### TriggerStudioModeTransition

Triggers the current scene transition. Same functionality as the `Transition` button in studio mode.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

---

### SetTBarPosition

Sets the position of the TBar.

**Very important note**: This will be deprecated and replaced in a future version of obs-websocket.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| position | Number | New position | >= 0.0, <= 1.0 | N/A |
| ?release | Boolean | Whether to release the TBar. Only set `false` if you know that you will be sending another position update | None | `true` |

## Filters Requests

### GetSourceFilterKindList

Gets an array of all available source filter kinds.

Similar to `GetInputKindList`

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.4.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| sourceFilterKinds | Array&lt;String&gt; | Array of source filter kinds |

---

### GetSourceFilterList

Gets an array of all of a source's filters.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sourceName | String | Name of the source | None | Unknown |
| ?sourceUuid | String | UUID of the source | None | Unknown |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| filters | Array&lt;Object&gt; | Array of filters |

---

### GetSourceFilterDefaultSettings

Gets the default settings for a filter kind.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| filterKind | String | Filter kind to get the default settings for | None | N/A |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| defaultFilterSettings | Object | Object of default settings for the filter kind |

---

### CreateSourceFilter

Creates a new filter, adding it to the specified source.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sourceName | String | Name of the source to add the filter to | None | Unknown |
| ?sourceUuid | String | UUID of the source to add the filter to | None | Unknown |
| filterName | String | Name of the new filter to be created | None | N/A |
| filterKind | String | The kind of filter to be created | None | N/A |
| ?filterSettings | Object | Settings object to initialize the filter with | None | Default settings used |

---

### RemoveSourceFilter

Removes a filter from a source.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sourceName | String | Name of the source the filter is on | None | Unknown |
| ?sourceUuid | String | UUID of the source the filter is on | None | Unknown |
| filterName | String | Name of the filter to remove | None | N/A |

---

### SetSourceFilterName

Sets the name of a source filter (rename).

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sourceName | String | Name of the source the filter is on | None | Unknown |
| ?sourceUuid | String | UUID of the source the filter is on | None | Unknown |
| filterName | String | Current name of the filter | None | N/A |
| newFilterName | String | New name for the filter | None | N/A |

---

### GetSourceFilter

Gets the info for a specific source filter.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sourceName | String | Name of the source | None | Unknown |
| ?sourceUuid | String | UUID of the source | None | Unknown |
| filterName | String | Name of the filter | None | N/A |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| filterEnabled | Boolean | Whether the filter is enabled |
| filterIndex | Number | Index of the filter in the list, beginning at 0 |
| filterKind | String | The kind of filter |
| filterSettings | Object | Settings object associated with the filter |

---

### SetSourceFilterIndex

Sets the index position of a filter on a source.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sourceName | String | Name of the source the filter is on | None | Unknown |
| ?sourceUuid | String | UUID of the source the filter is on | None | Unknown |
| filterName | String | Name of the filter | None | N/A |
| filterIndex | Number | New index position of the filter | >= 0 | N/A |

---

### SetSourceFilterSettings

Sets the settings of a source filter.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sourceName | String | Name of the source the filter is on | None | Unknown |
| ?sourceUuid | String | UUID of the source the filter is on | None | Unknown |
| filterName | String | Name of the filter to set the settings of | None | N/A |
| filterSettings | Object | Object of settings to apply | None | N/A |
| ?overlay | Boolean | True == apply the settings on top of existing ones, False == reset the input to its defaults, then apply settings. | None | true |

---

### SetSourceFilterEnabled

Sets the enable state of a source filter.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sourceName | String | Name of the source the filter is on | None | Unknown |
| ?sourceUuid | String | UUID of the source the filter is on | None | Unknown |
| filterName | String | Name of the filter | None | N/A |
| filterEnabled | Boolean | New enable state of the filter | None | N/A |

## Scene Items Requests

### GetSceneItemList

Gets a list of all scene items in a scene.

Scenes only

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene to get the items of | None | Unknown |
| ?sceneUuid | String | UUID of the scene to get the items of | None | Unknown |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| sceneItems | Array&lt;Object&gt; | Array of scene items in the scene |

---

### GetGroupSceneItemList

Basically GetSceneItemList, but for groups.

Using groups at all in OBS is discouraged, as they are very broken under the hood. Please use nested scenes instead.

Groups only

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the group to get the items of | None | Unknown |
| ?sceneUuid | String | UUID of the group to get the items of | None | Unknown |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| sceneItems | Array&lt;Object&gt; | Array of scene items in the group |

---

### GetSceneItemId

Searches a scene for a source, and returns its id.

Scenes and Groups

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene or group to search in | None | Unknown |
| ?sceneUuid | String | UUID of the scene or group to search in | None | Unknown |
| sourceName | String | Name of the source to find | None | N/A |
| ?searchOffset | Number | Number of matches to skip during search. >= 0 means first forward. -1 means last (top) item | >= -1 | 0 |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| sceneItemId | Number | Numeric ID of the scene item |

---

### GetSceneItemSource

Gets the source associated with a scene item.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.4.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene the item is in | None | Unknown |
| ?sceneUuid | String | UUID of the scene the item is in | None | Unknown |
| sceneItemId | Number | Numeric ID of the scene item | >= 0 | N/A |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| sourceName | String | Name of the source associated with the scene item |
| sourceUuid | String | UUID of the source associated with the scene item |

---

### CreateSceneItem

Creates a new scene item using a source.

Scenes only

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene to create the new item in | None | Unknown |
| ?sceneUuid | String | UUID of the scene to create the new item in | None | Unknown |
| ?sourceName | String | Name of the source to add to the scene | None | Unknown |
| ?sourceUuid | String | UUID of the source to add to the scene | None | Unknown |
| ?sceneItemEnabled | Boolean | Enable state to apply to the scene item on creation | None | True |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| sceneItemId | Number | Numeric ID of the scene item |

---

### RemoveSceneItem

Removes a scene item from a scene.

Scenes only

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene the item is in | None | Unknown |
| ?sceneUuid | String | UUID of the scene the item is in | None | Unknown |
| sceneItemId | Number | Numeric ID of the scene item | >= 0 | N/A |

---

### DuplicateSceneItem

Duplicates a scene item, copying all transform and crop info.

Scenes only

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene the item is in | None | Unknown |
| ?sceneUuid | String | UUID of the scene the item is in | None | Unknown |
| sceneItemId | Number | Numeric ID of the scene item | >= 0 | N/A |
| ?destinationSceneName | String | Name of the scene to create the duplicated item in | None | From scene is assumed |
| ?destinationSceneUuid | String | UUID of the scene to create the duplicated item in | None | From scene is assumed |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| sceneItemId | Number | Numeric ID of the duplicated scene item |

---

### GetSceneItemTransform

Gets the transform and crop info of a scene item.

Scenes and Groups

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene the item is in | None | Unknown |
| ?sceneUuid | String | UUID of the scene the item is in | None | Unknown |
| sceneItemId | Number | Numeric ID of the scene item | >= 0 | N/A |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| sceneItemTransform | Object | Object containing scene item transform info |

---

### SetSceneItemTransform

Sets the transform and crop info of a scene item.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene the item is in | None | Unknown |
| ?sceneUuid | String | UUID of the scene the item is in | None | Unknown |
| sceneItemId | Number | Numeric ID of the scene item | >= 0 | N/A |
| sceneItemTransform | Object | Object containing scene item transform info to update | None | N/A |

---

### GetSceneItemEnabled

Gets the enable state of a scene item.

Scenes and Groups

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene the item is in | None | Unknown |
| ?sceneUuid | String | UUID of the scene the item is in | None | Unknown |
| sceneItemId | Number | Numeric ID of the scene item | >= 0 | N/A |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| sceneItemEnabled | Boolean | Whether the scene item is enabled. `true` for enabled, `false` for disabled |

---

### SetSceneItemEnabled

Sets the enable state of a scene item.

Scenes and Groups

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene the item is in | None | Unknown |
| ?sceneUuid | String | UUID of the scene the item is in | None | Unknown |
| sceneItemId | Number | Numeric ID of the scene item | >= 0 | N/A |
| sceneItemEnabled | Boolean | New enable state of the scene item | None | N/A |

---

### GetSceneItemLocked

Gets the lock state of a scene item.

Scenes and Groups

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene the item is in | None | Unknown |
| ?sceneUuid | String | UUID of the scene the item is in | None | Unknown |
| sceneItemId | Number | Numeric ID of the scene item | >= 0 | N/A |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| sceneItemLocked | Boolean | Whether the scene item is locked. `true` for locked, `false` for unlocked |

---

### SetSceneItemLocked

Sets the lock state of a scene item.

Scenes and Group

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene the item is in | None | Unknown |
| ?sceneUuid | String | UUID of the scene the item is in | None | Unknown |
| sceneItemId | Number | Numeric ID of the scene item | >= 0 | N/A |
| sceneItemLocked | Boolean | New lock state of the scene item | None | N/A |

---

### GetSceneItemIndex

Gets the index position of a scene item in a scene.

An index of 0 is at the bottom of the source list in the UI.

Scenes and Groups

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene the item is in | None | Unknown |
| ?sceneUuid | String | UUID of the scene the item is in | None | Unknown |
| sceneItemId | Number | Numeric ID of the scene item | >= 0 | N/A |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| sceneItemIndex | Number | Index position of the scene item |

---

### SetSceneItemIndex

Sets the index position of a scene item in a scene.

Scenes and Groups

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene the item is in | None | Unknown |
| ?sceneUuid | String | UUID of the scene the item is in | None | Unknown |
| sceneItemId | Number | Numeric ID of the scene item | >= 0 | N/A |
| sceneItemIndex | Number | New index position of the scene item | >= 0 | N/A |

---

### GetSceneItemBlendMode

Gets the blend mode of a scene item.

Blend modes:

- `OBS_BLEND_NORMAL`
- `OBS_BLEND_ADDITIVE`
- `OBS_BLEND_SUBTRACT`
- `OBS_BLEND_SCREEN`
- `OBS_BLEND_MULTIPLY`
- `OBS_BLEND_LIGHTEN`
- `OBS_BLEND_DARKEN`

Scenes and Groups

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene the item is in | None | Unknown |
| ?sceneUuid | String | UUID of the scene the item is in | None | Unknown |
| sceneItemId | Number | Numeric ID of the scene item | >= 0 | N/A |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| sceneItemBlendMode | String | Current blend mode |

---

### SetSceneItemBlendMode

Sets the blend mode of a scene item.

Scenes and Groups

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sceneName | String | Name of the scene the item is in | None | Unknown |
| ?sceneUuid | String | UUID of the scene the item is in | None | Unknown |
| sceneItemId | Number | Numeric ID of the scene item | >= 0 | N/A |
| sceneItemBlendMode | String | New blend mode | None | N/A |

## Outputs Requests

### GetVirtualCamStatus

Gets the status of the virtualcam output.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| outputActive | Boolean | Whether the output is active |

---

### ToggleVirtualCam

Toggles the state of the virtualcam output.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| outputActive | Boolean | Whether the output is active |

---

### StartVirtualCam

Starts the virtualcam output.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

---

### StopVirtualCam

Stops the virtualcam output.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

---

### GetReplayBufferStatus

Gets the status of the replay buffer output.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| outputActive | Boolean | Whether the output is active |

---

### ToggleReplayBuffer

Toggles the state of the replay buffer output.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| outputActive | Boolean | Whether the output is active |

---

### StartReplayBuffer

Starts the replay buffer output.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

---

### StopReplayBuffer

Stops the replay buffer output.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

---

### SaveReplayBuffer

Saves the contents of the replay buffer output.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

---

### GetLastReplayBufferReplay

Gets the filename of the last replay buffer save file.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| savedReplayPath | String | File path |

---

### GetOutputList

Gets the list of available outputs.

- Complexity Rating: `4/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| outputs | Array&lt;Object&gt; | Array of outputs |

---

### GetOutputStatus

Gets the status of an output.

- Complexity Rating: `4/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| outputName | String | Output name | None | N/A |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| outputActive | Boolean | Whether the output is active |
| outputReconnecting | Boolean | Whether the output is reconnecting |
| outputTimecode | String | Current formatted timecode string for the output |
| outputDuration | Number | Current duration in milliseconds for the output |
| outputCongestion | Number | Congestion of the output |
| outputBytes | Number | Number of bytes sent by the output |
| outputSkippedFrames | Number | Number of frames skipped by the output's process |
| outputTotalFrames | Number | Total number of frames delivered by the output's process |

---

### ToggleOutput

Toggles the status of an output.

- Complexity Rating: `4/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| outputName | String | Output name | None | N/A |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| outputActive | Boolean | Whether the output is active |

---

### StartOutput

Starts an output.

- Complexity Rating: `4/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| outputName | String | Output name | None | N/A |

---

### StopOutput

Stops an output.

- Complexity Rating: `4/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| outputName | String | Output name | None | N/A |

---

### GetOutputSettings

Gets the settings of an output.

- Complexity Rating: `4/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| outputName | String | Output name | None | N/A |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| outputSettings | Object | Output settings |

---

### SetOutputSettings

Sets the settings of an output.

- Complexity Rating: `4/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| outputName | String | Output name | None | N/A |
| outputSettings | Object | Output settings | None | N/A |

## Stream Requests

### GetStreamStatus

Gets the status of the stream output.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| outputActive | Boolean | Whether the output is active |
| outputReconnecting | Boolean | Whether the output is currently reconnecting |
| outputTimecode | String | Current formatted timecode string for the output |
| outputDuration | Number | Current duration in milliseconds for the output |
| outputCongestion | Number | Congestion of the output |
| outputBytes | Number | Number of bytes sent by the output |
| outputSkippedFrames | Number | Number of frames skipped by the output's process |
| outputTotalFrames | Number | Total number of frames delivered by the output's process |

---

### ToggleStream

Toggles the status of the stream output.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| outputActive | Boolean | New state of the stream output |

---

### StartStream

Starts the stream output.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

---

### StopStream

Stops the stream output.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

---

### SendStreamCaption

Sends CEA-608 caption text over the stream output.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| captionText | String | Caption text | None | N/A |

## Record Requests

### GetRecordStatus

Gets the status of the record output.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| outputActive | Boolean | Whether the output is active |
| outputPaused | Boolean | Whether the output is paused |
| outputTimecode | String | Current formatted timecode string for the output |
| outputDuration | Number | Current duration in milliseconds for the output |
| outputBytes | Number | Number of bytes sent by the output |

---

### ToggleRecord

Toggles the status of the record output.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| outputActive | Boolean | The new active state of the output |

---

### StartRecord

Starts the record output.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

---

### StopRecord

Stops the record output.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| outputPath | String | File name for the saved recording |

---

### ToggleRecordPause

Toggles pause on the record output.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

---

### PauseRecord

Pauses the record output.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

---

### ResumeRecord

Resumes the record output.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

---

### SplitRecordFile

Splits the current file being recorded into a new file.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.5.0

---

### CreateRecordChapter

Adds a new chapter marker to the file currently being recorded.

Note: As of OBS 30.2.0, the only file format supporting this feature is Hybrid MP4.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.5.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?chapterName | String | Name of the new chapter | None | Unknown |

## Media Inputs Requests

### GetMediaInputStatus

Gets the status of a media input.

Media States:

- `OBS_MEDIA_STATE_NONE`
- `OBS_MEDIA_STATE_PLAYING`
- `OBS_MEDIA_STATE_OPENING`
- `OBS_MEDIA_STATE_BUFFERING`
- `OBS_MEDIA_STATE_PAUSED`
- `OBS_MEDIA_STATE_STOPPED`
- `OBS_MEDIA_STATE_ENDED`
- `OBS_MEDIA_STATE_ERROR`

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the media input | None | Unknown |
| ?inputUuid | String | UUID of the media input | None | Unknown |

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| mediaState | String | State of the media input |
| mediaDuration | Number | Total duration of the playing media in milliseconds. `null` if not playing |
| mediaCursor | Number | Position of the cursor in milliseconds. `null` if not playing |

---

### SetMediaInputCursor

Sets the cursor position of a media input.

This request does not perform bounds checking of the cursor position.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the media input | None | Unknown |
| ?inputUuid | String | UUID of the media input | None | Unknown |
| mediaCursor | Number | New cursor position to set | >= 0 | N/A |

---

### OffsetMediaInputCursor

Offsets the current cursor position of a media input by the specified value.

This request does not perform bounds checking of the cursor position.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the media input | None | Unknown |
| ?inputUuid | String | UUID of the media input | None | Unknown |
| mediaCursorOffset | Number | Value to offset the current cursor position by | None | N/A |

---

### TriggerMediaInputAction

Triggers an action on a media input.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the media input | None | Unknown |
| ?inputUuid | String | UUID of the media input | None | Unknown |
| mediaAction | String | Identifier of the `ObsMediaInputAction` enum | None | N/A |

## Ui Requests

### GetStudioModeEnabled

Gets whether studio is enabled.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| studioModeEnabled | Boolean | Whether studio mode is enabled |

---

### SetStudioModeEnabled

Enables or disables studio mode

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| studioModeEnabled | Boolean | True == Enabled, False == Disabled | None | N/A |

---

### OpenInputPropertiesDialog

Opens the properties dialog of an input.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input to open the dialog of | None | Unknown |
| ?inputUuid | String | UUID of the input to open the dialog of | None | Unknown |

---

### OpenInputFiltersDialog

Opens the filters dialog of an input.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input to open the dialog of | None | Unknown |
| ?inputUuid | String | UUID of the input to open the dialog of | None | Unknown |

---

### OpenInputInteractDialog

Opens the interact dialog of an input.

- Complexity Rating: `1/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?inputName | String | Name of the input to open the dialog of | None | Unknown |
| ?inputUuid | String | UUID of the input to open the dialog of | None | Unknown |

---

### GetMonitorList

Gets a list of connected monitors and information about them.

- Complexity Rating: `2/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Response Fields:**

| Name | Type  | Description |
| ---- | :---: | ----------- |
| monitors | Array&lt;Object&gt; | a list of detected monitors with some information |

---

### OpenVideoMixProjector

Opens a projector for a specific output video mix.

Mix types:

- `OBS_WEBSOCKET_VIDEO_MIX_TYPE_PREVIEW`
- `OBS_WEBSOCKET_VIDEO_MIX_TYPE_PROGRAM`
- `OBS_WEBSOCKET_VIDEO_MIX_TYPE_MULTIVIEW`

Note: This request serves to provide feature parity with 4.x. It is very likely to be changed/deprecated in a future release.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| videoMixType | String | Type of mix to open | None | N/A |
| ?monitorIndex | Number | Monitor index, use `GetMonitorList` to obtain index | None | -1: Opens projector in windowed mode |
| ?projectorGeometry | String | Size/Position data for a windowed projector, in Qt Base64 encoded format. Mutually exclusive with `monitorIndex` | None | N/A |

---

### OpenSourceProjector

Opens a projector for a source.

Note: This request serves to provide feature parity with 4.x. It is very likely to be changed/deprecated in a future release.

- Complexity Rating: `3/5`
- Latest Supported RPC Version: `1`
- Added in v5.0.0

**Request Fields:**

| Name | Type  | Description | Value Restrictions | ?Default Behavior |
| ---- | :---: | ----------- | :----------------: | ----------------- |
| ?sourceName | String | Name of the source to open a projector for | None | Unknown |
| ?sourceUuid | String | UUID of the source to open a projector for | None | Unknown |
| ?monitorIndex | Number | Monitor index, use `GetMonitorList` to obtain index | None | -1: Opens projector in windowed mode |
| ?projectorGeometry | String | Size/Position data for a windowed projector, in Qt Base64 encoded format. Mutually exclusive with `monitorIndex` | None | N/A |
