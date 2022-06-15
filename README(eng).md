# Unity UI Rotation
This module is designed to respond to changes in the layout and size of the UI when the screen is in landscape mode or portrait mode.
After completing the UI task in horizontal resolution, save it. After completing the UI task in vertical resolution, save it too.
And then, automatically set the UI to match the aspect resolution when the screen is rotated.

# How to use

## Preparation
- Add "Ignore" tag from tag
- Attaching a 'ComponentsManager' script to an empty object

## Save UI Data
![화면 캡처 2021-04-19 094043](https://user-images.githubusercontent.com/75019048/115168549-4da52680-a0f6-11eb-9644-b65024b0d5c2.jpg)
1. Attach 'ComponentProperty' script to target Canvas
2. Proceed with UI screen operation in landscape or portrait resolution
3. Click the 'Save' button on the Component menu in Component Property (or enter the 'shift + s' shortcut)
4. `Assets/Resources/JsonData/{Orientation}/{SceneName}/{CanvasName}` 경로에 저장 완료

## Load
### Load from Editor Mode
1. Change to landscape or portrait resolution
2. Click the object with the 'ComponentsManager' script attached in the high-key window and enter the 'shift + l' shortcut
### Calling up during play mode
1. Click the Play button
2. Changing the resolution automatically brings up the UI screen that you worked with that resolution

## Sample
![샘플](https://github.com/Joseph-Cha/UnityRotateUI/blob/main/GIF/Sample.gif?raw=true)
