# VRControllerHint
Onboarding VR controllers by highlighting the buttons in Unity game engine. 

Quickly add controller button Hints to your VR applications using this asset. It is independant of XR SDK and so it should work with OpenXR, Unity XR, Legacy VR including Oculus Integration and SteamVR. 

Supported devices – Oculus Rift, Oculus Quest, HTC Vive, Microsoft MR.

![VRControllerHint](https://github.com/sivabalan-m/VRControllerHint/assets/43854177/ac4d711d-e6a1-42a0-8cf1-23af78b1fdbd)


**How to setup:**
1.	Import the “Quick Outline” free asset from asset store: https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488. This is already included in the repo.
2.	Drag and drop the “LeftControllers” and “RightControllers” prefabs as child of respective hand anchors of your XR Rig or Player.
3.	Add the component “ControllerButton.cs” on your XR Rig or Player.
4.	That’s it! You are good to go.

Unity XR – Demo Scene:
- Refer the “ControllerHintDemo_UnityXR” scene under ‘Demo’ folder.
- You need to install ‘XR Interaction Toolkit’ from the Package Manager to run this demo.

Oculus Integration – Where to add controller prefabs:

![image](https://github.com/sivabalan-m/VRControllerHint/assets/43854177/d86e2baf-46e6-4aba-a55a-9098b1f8ccd0)

SteamVR – Where to add controller prefabs

![image](https://github.com/sivabalan-m/VRControllerHint/assets/43854177/7029f46e-b364-4718-9d0f-bf1230609b4f)

**ControllerButton.cs – Public Booleans:**
- isColor – if enabled, the matrial color of the respective buttons will change. By default, The material color will change to Yellow. If you want a custom color, go to VRControllerHint/Models/ColorMaterials folder. Select all materials and change to your desired material albedo color.
- isHighlight – if enabled, the respective button will be highlighted with the quick outline shader.
- isHighlightAnimate – if enabled, the Highlight outline width will animate.
- isHaptics – if enabled, the respective controller will receive haptic vibrations

**Scripts to call controller Hints:**

Show a Controller Hint for a button:
```csharp
ControllerButton.ShowButtonHint(ControllerButton.Hand.Left, ControllerButton.Buttons.Grip, "Click this button to grab the object");
```
Hide a Controller Hint for a button:
```csharp
ControllerButton.HideButtonHint(ControllerButton.Hand.Left, ControllerButton.Buttons.Grip);
```
Show All Controller Hints:
```csharp
ControllerButton.ShowAllHint(ControllerButton.Hand.Left);
```
Hide All Controller Hints:
```csharp
ControllerButton.HideAllHint(ControllerButton.Hand.Left);
```



