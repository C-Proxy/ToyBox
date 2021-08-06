// GENERATED AUTOMATICALLY FROM 'Assets/Main/Setting/OVRPlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @OVRPlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @OVRPlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""OVRPlayerInput"",
    ""maps"": [
        {
            ""name"": ""LeftInput"",
            ""id"": ""7d16f8e1-0c3b-4e1a-8010-cb809d7c95a2"",
            ""actions"": [
                {
                    ""name"": ""StickAxis"",
                    ""type"": ""Value"",
                    ""id"": ""09111458-5957-4dae-bcbc-db53cd46a9f5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StickClick"",
                    ""type"": ""Button"",
                    ""id"": ""ba8233c5-82f0-4fc2-a566-a937d8523112"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""MultiTap""
                },
                {
                    ""name"": ""StickTouch"",
                    ""type"": ""Button"",
                    ""id"": ""8ee90827-a92f-4d9f-b922-657f01471b23"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ThumbTouch"",
                    ""type"": ""Button"",
                    ""id"": ""8b30e1f7-dea1-4d32-b11e-9438d80c556e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MainPress"",
                    ""type"": ""Button"",
                    ""id"": ""2f85cc4c-7844-49b9-8a61-854ba1a7fdc2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""MainClick"",
                    ""type"": ""Button"",
                    ""id"": ""14915f4b-1bd3-4ca6-93cf-3049b2d2e02e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""MultiTap""
                },
                {
                    ""name"": ""MainTouch"",
                    ""type"": ""Button"",
                    ""id"": ""a5a620bc-ff0b-43e3-954a-5b166f9b68c4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SubPress"",
                    ""type"": ""Button"",
                    ""id"": ""5f75eac5-67ce-49a6-95f6-8f7589b663b8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""SubClick"",
                    ""type"": ""Button"",
                    ""id"": ""0d5ffe1d-2f20-4877-9bc4-d9ee171cd1ee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""MultiTap""
                },
                {
                    ""name"": ""SubTouch"",
                    ""type"": ""Button"",
                    ""id"": ""c3dd26d0-6455-4a59-b798-d8dfd89e73fe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""IndexTrigger"",
                    ""type"": ""Value"",
                    ""id"": ""37140926-951c-4fc9-9fa5-b7d75480c7f2"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""IndexPress"",
                    ""type"": ""Button"",
                    ""id"": ""e930875b-ce4f-471b-baa8-9690ab72e1a4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""IndexClick"",
                    ""type"": ""Button"",
                    ""id"": ""b244f067-dbcf-458d-b10d-3f9a9ff46b0f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""MultiTap""
                },
                {
                    ""name"": ""IndexTouch"",
                    ""type"": ""Button"",
                    ""id"": ""532fe2ec-39a0-404d-9ad7-57e040252fde"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""HandTrigger"",
                    ""type"": ""Value"",
                    ""id"": ""44919c47-85c9-4e31-a6c9-ca6c29885406"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""HandPress"",
                    ""type"": ""Button"",
                    ""id"": ""fb999109-5cc2-4b50-af52-177de9694114"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""978bfe49-cc26-4a3d-ab7b-7d7a29327403"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""StickAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""00ca640b-d935-4593-8157-c05846ea39b3"",
                    ""path"": ""Dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StickAxis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e2062cb9-1b15-46a2-838c-2f8d72a0bdd9"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""StickAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8180e8bd-4097-4f4e-ab88-4523101a6ce9"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""StickAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""320bffee-a40b-4347-ac70-c210eb8bc73a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""StickAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""1c5327b5-f71c-4f60-99c7-4e737386f1d1"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""StickAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d2581a9b-1d11-4566-b27d-b92aff5fabbc"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""StickAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2e46982e-44cc-431b-9f0b-c11910bf467a"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""StickAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fcfe95b8-67b9-4526-84b5-5d0bc98d6400"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""StickAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""77bff152-3580-4b21-b6de-dcd0c7e41164"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""StickAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1635d3fe-58b6-4ba9-a4e2-f4b964f6b5c8"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerLeft>{LeftHand}/thumbstick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""StickAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ea4d645-4504-4529-b061-ab81934c3752"",
                    ""path"": ""<Joystick>/stick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""StickAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bcaba163-7b64-4dfc-a0a8-5db6b3dbb2db"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerLeft>{LeftHand}/primarytouched"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""MainTouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""55a6ba9b-fced-4054-af9b-e425c164002d"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerLeft>{LeftHand}/thumbstickclicked"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""StickClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1b61d632-bfdb-4058-8101-374953247e03"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerLeft>{LeftHand}/thumbsticktouched"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""StickTouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6f534cf4-abfe-4ccc-bcbc-397da477036c"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerLeft>{LeftHand}/primarybutton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""MainPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2c21411e-50f6-4c22-b0d3-fa4a1254a267"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerLeft>{LeftHand}/secondarybutton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""SubPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a51797fc-30a0-4af5-bf02-804dbe6cd778"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerLeft>{LeftHand}/secondarytouched"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""SubTouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dce761d8-9033-4047-9a66-dc60d9a1ac4a"",
                    ""path"": ""<XRController>{LeftHand}/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""IndexTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f55a9946-92e5-494c-a9df-ba2892363a1a"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{LeftHand}/triggerpressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""IndexPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d0868fc-06d1-413a-ad65-465d968f08f6"",
                    ""path"": ""<OculusTouchController>{LeftHand}/grip"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""HandTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""577f82f4-a10f-48d3-a36e-c850ab0a83c5"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerLeft>{LeftHand}/grippressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""HandPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d39dea00-8a08-4761-865c-4839f47a4665"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{LeftHand}/triggertouched"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""IndexTouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""df7836fe-3e35-4b2a-bd54-a0c2a321e829"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{LeftHand}/thumbtouch"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""ThumbTouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2d74bdde-b591-4abe-9bf3-bf85b2086951"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerLeft>{LeftHand}/primarybutton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""MainClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7dabb07c-a7b2-4c18-8f5f-fe7cd33b0e15"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerLeft>{LeftHand}/secondarybutton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""SubClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d83b7889-5c0b-4f68-b812-c0c39a181125"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{LeftHand}/triggerpressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""IndexClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""RightInput"",
            ""id"": ""7d3f58d9-be39-411a-a71f-000da06c2fad"",
            ""actions"": [
                {
                    ""name"": ""StickAxis"",
                    ""type"": ""Value"",
                    ""id"": ""0f1ed1e1-4de5-4d19-bb44-c439db001bce"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StickClick"",
                    ""type"": ""Button"",
                    ""id"": ""ddd17568-8f4e-4882-81ba-cb81261a4174"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""MultiTap""
                },
                {
                    ""name"": ""StickTouch"",
                    ""type"": ""Button"",
                    ""id"": ""556e73ae-a8c0-4a0d-8e66-189370b5c862"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ThumbTouch"",
                    ""type"": ""Button"",
                    ""id"": ""38918398-e00d-44d3-9e40-f61066e3da28"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MainPress"",
                    ""type"": ""Button"",
                    ""id"": ""5766918b-87f9-4240-af61-f4fc2894d0f4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""MainClick"",
                    ""type"": ""Button"",
                    ""id"": ""d5a25792-9f8c-4dd2-af6f-bc1aa97f11e5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""MultiTap""
                },
                {
                    ""name"": ""MainTouch"",
                    ""type"": ""Button"",
                    ""id"": ""cca0012b-7629-4ef3-8256-12c8367ec5c0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SubPress"",
                    ""type"": ""Button"",
                    ""id"": ""fe38d3a7-de81-4e13-a5ae-462319aa3c2d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""SubClick"",
                    ""type"": ""Button"",
                    ""id"": ""4e0abdf4-92c7-4d42-ae8f-583cff215072"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""MultiTap""
                },
                {
                    ""name"": ""SubTouch"",
                    ""type"": ""Button"",
                    ""id"": ""49141fb9-381a-4157-8f69-667c8facf7ff"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""IndexTrigger"",
                    ""type"": ""Value"",
                    ""id"": ""c39ab9bd-7674-4554-be1a-ade766530236"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""IndexPress"",
                    ""type"": ""Button"",
                    ""id"": ""0a500288-78eb-490f-8318-003fdc5c955a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""IndexClick"",
                    ""type"": ""Button"",
                    ""id"": ""057e4e76-3494-4d7f-88c6-8faac61aa51e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""MultiTap""
                },
                {
                    ""name"": ""IndexTouch"",
                    ""type"": ""Button"",
                    ""id"": ""7152d9cd-602a-4e70-8973-05d43b014729"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""HandTrigger"",
                    ""type"": ""Value"",
                    ""id"": ""87d23760-7e16-405f-8f92-b7f4510374e8"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""HandPress"",
                    ""type"": ""Button"",
                    ""id"": ""5b330d29-53bf-4889-ada3-6040b0106e68"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d0724416-3095-4c5e-bbb1-703119d2de4c"",
                    ""path"": ""<OculusTouchController>{RightHand}/thumbstick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""StickAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""735e605d-ddd5-4451-978b-e72d5711b4d1"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/thumbstickclicked"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""StickClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4a55a924-b3a6-446e-81e6-ac418b1f7a1f"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/thumbsticktouched"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""StickTouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""89ad7d7e-647a-4d0c-b036-b6eeba69ea6f"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/grippressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""HandPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3e9c8e61-ba3d-40e5-aedb-db153ff380d1"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/primarybutton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""MainPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c72facf9-ea14-4ec2-928e-058bd5c80754"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/primarytouched"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""MainTouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""357cd553-953d-4214-beef-08446e36a1fc"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/secondarybutton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""SubPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""471926df-b696-4de3-871a-7accc9368c9c"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/secondarytouched"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""SubTouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""afa110c0-16c8-418e-9085-048d2a743e6a"",
                    ""path"": ""<OculusTouchController>{RightHand}/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""IndexTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7531e7e-0349-4669-9b68-4855cdcb7cda"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/triggerpressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""IndexPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""63b91f92-2634-4079-8be5-ae510a067279"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/triggerpressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""IndexTouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""57b23260-d324-4156-88c9-1e7c737f2dea"",
                    ""path"": ""<OculusTouchController>{RightHand}/grip"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""HandTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""11cfdb50-158f-4a60-8813-6e7694b04871"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/thumbresttouch"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""ThumbTouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1ba39f46-e2bf-4d2d-af4e-d0e6da51fab8"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/primarybutton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""MainClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""948d1053-449b-4070-866d-15ff5b1a22fd"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/secondarybutton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""SubClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9efb1620-69b9-4394-b3b8-bb700729dd10"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/triggerpressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""IndexClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""726cc2d6-cb2e-4839-bd8d-39ce73bbd129"",
            ""actions"": [
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Value"",
                    ""id"": ""dc7c8a19-2c57-4dfd-bcfc-cf8ff28bbc77"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""31622634-4999-46be-b37c-ad5038ea22c8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""c83845db-5943-43c4-96c1-4eece068a78a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""cbf9af38-626c-4b87-a1d8-acea362df445"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a643b6f8-af24-4dd6-88e0-4bd8e52a2bd1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ScrollWheel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""dbfbd251-7f28-461e-a4c3-3c0ff661f9a2"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MiddleClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""bceaf019-db8b-407b-85f4-bf894d1b87f5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f8e07747-b6fb-44c6-87ac-2e8ccac18b75"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrackedDevicePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""4ee49375-63f4-4ed6-bf2b-0c9eea631aa0"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrackedDeviceOrientation"",
                    ""type"": ""PassThrough"",
                    ""id"": ""242a7228-0697-4e03-b86e-14d0dd4eb253"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""809f371f-c5e2-4e7a-83a1-d867598f40dd"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""14a5d6e8-4aaf-4119-a9ef-34b8c2c548bf"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9144cbe6-05e1-4687-a6d7-24f99d23dd81"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2db08d65-c5fb-421b-983f-c71163608d67"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""58748904-2ea9-4a80-8579-b500e6a76df8"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8ba04515-75aa-45de-966d-393d9bbd1c14"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""712e721c-bdfb-4b23-a86c-a0d9fcfea921"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fcd248ae-a788-4676-a12e-f4d81205600b"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1f04d9bc-c50b-41a1-bfcc-afb75475ec20"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""fb8277d4-c5cd-4663-9dc7-ee3f0b506d90"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Joystick"",
                    ""id"": ""e25d9774-381c-4a61-b47c-7b6b299ad9f9"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3db53b26-6601-41be-9887-63ac74e79d19"",
                    ""path"": ""<Joystick>/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0cb3e13e-3d90-4178-8ae6-d9c5501d653f"",
                    ""path"": ""<Joystick>/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0392d399-f6dd-4c82-8062-c1e9c0d34835"",
                    ""path"": ""<Joystick>/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""942a66d9-d42f-43d6-8d70-ecb4ba5363bc"",
                    ""path"": ""<Joystick>/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""ff527021-f211-4c02-933e-5976594c46ed"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""563fbfdd-0f09-408d-aa75-8642c4f08ef0"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""eb480147-c587-4a33-85ed-eb0ab9942c43"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2bf42165-60bc-42ca-8072-8c13ab40239b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""85d264ad-e0a0-4565-b7ff-1a37edde51ac"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""74214943-c580-44e4-98eb-ad7eebe17902"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""cea9b045-a000-445b-95b8-0c171af70a3b"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8607c725-d935-4808-84b1-8354e29bab63"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4cda81dc-9edd-4e03-9d7c-a71a14345d0b"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9e92bb26-7e3b-4ec4-b06b-3c8f8e498ddc"",
                    ""path"": ""*/{Submit}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82627dcc-3b13-4ba9-841d-e4b746d6553e"",
                    ""path"": ""*/{Cancel}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c52c8e0b-8179-41d3-b8a1-d149033bbe86"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e1394cbc-336e-44ce-9ea8-6007ed6193f7"",
                    ""path"": ""<Pen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5693e57a-238a-46ed-b5ae-e64e6e574302"",
                    ""path"": ""<Touchscreen>/touch*/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4faf7dc9-b979-4210-aa8c-e808e1ef89f5"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d66d5ba-88d7-48e6-b1cd-198bbfef7ace"",
                    ""path"": ""<Pen>/tip"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""47c2a644-3ebc-4dae-a106-589b7ca75b59"",
                    ""path"": ""<Touchscreen>/touch*/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb9e6b34-44bf-4381-ac63-5aa15d19f677"",
                    ""path"": ""<XRController>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38c99815-14ea-4617-8627-164d27641299"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""ScrollWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24066f69-da47-44f3-a07e-0015fb02eb2e"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""MiddleClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c191405-5738-4d4b-a523-c6a301dbf754"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7236c0d9-6ca3-47cf-a6ee-a97f5b59ea77"",
                    ""path"": ""<XRController>/devicePosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TrackedDevicePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23e01e3a-f935-4948-8d8b-9bcac77714fb"",
                    ""path"": ""<XRController>/deviceRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TrackedDeviceOrientation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Joystick"",
            ""bindingGroup"": ""Joystick"",
            ""devices"": [
                {
                    ""devicePath"": ""<Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XR"",
            ""bindingGroup"": ""XR"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // LeftInput
        m_LeftInput = asset.FindActionMap("LeftInput", throwIfNotFound: true);
        m_LeftInput_StickAxis = m_LeftInput.FindAction("StickAxis", throwIfNotFound: true);
        m_LeftInput_StickClick = m_LeftInput.FindAction("StickClick", throwIfNotFound: true);
        m_LeftInput_StickTouch = m_LeftInput.FindAction("StickTouch", throwIfNotFound: true);
        m_LeftInput_ThumbTouch = m_LeftInput.FindAction("ThumbTouch", throwIfNotFound: true);
        m_LeftInput_MainPress = m_LeftInput.FindAction("MainPress", throwIfNotFound: true);
        m_LeftInput_MainClick = m_LeftInput.FindAction("MainClick", throwIfNotFound: true);
        m_LeftInput_MainTouch = m_LeftInput.FindAction("MainTouch", throwIfNotFound: true);
        m_LeftInput_SubPress = m_LeftInput.FindAction("SubPress", throwIfNotFound: true);
        m_LeftInput_SubClick = m_LeftInput.FindAction("SubClick", throwIfNotFound: true);
        m_LeftInput_SubTouch = m_LeftInput.FindAction("SubTouch", throwIfNotFound: true);
        m_LeftInput_IndexTrigger = m_LeftInput.FindAction("IndexTrigger", throwIfNotFound: true);
        m_LeftInput_IndexPress = m_LeftInput.FindAction("IndexPress", throwIfNotFound: true);
        m_LeftInput_IndexClick = m_LeftInput.FindAction("IndexClick", throwIfNotFound: true);
        m_LeftInput_IndexTouch = m_LeftInput.FindAction("IndexTouch", throwIfNotFound: true);
        m_LeftInput_HandTrigger = m_LeftInput.FindAction("HandTrigger", throwIfNotFound: true);
        m_LeftInput_HandPress = m_LeftInput.FindAction("HandPress", throwIfNotFound: true);
        // RightInput
        m_RightInput = asset.FindActionMap("RightInput", throwIfNotFound: true);
        m_RightInput_StickAxis = m_RightInput.FindAction("StickAxis", throwIfNotFound: true);
        m_RightInput_StickClick = m_RightInput.FindAction("StickClick", throwIfNotFound: true);
        m_RightInput_StickTouch = m_RightInput.FindAction("StickTouch", throwIfNotFound: true);
        m_RightInput_ThumbTouch = m_RightInput.FindAction("ThumbTouch", throwIfNotFound: true);
        m_RightInput_MainPress = m_RightInput.FindAction("MainPress", throwIfNotFound: true);
        m_RightInput_MainClick = m_RightInput.FindAction("MainClick", throwIfNotFound: true);
        m_RightInput_MainTouch = m_RightInput.FindAction("MainTouch", throwIfNotFound: true);
        m_RightInput_SubPress = m_RightInput.FindAction("SubPress", throwIfNotFound: true);
        m_RightInput_SubClick = m_RightInput.FindAction("SubClick", throwIfNotFound: true);
        m_RightInput_SubTouch = m_RightInput.FindAction("SubTouch", throwIfNotFound: true);
        m_RightInput_IndexTrigger = m_RightInput.FindAction("IndexTrigger", throwIfNotFound: true);
        m_RightInput_IndexPress = m_RightInput.FindAction("IndexPress", throwIfNotFound: true);
        m_RightInput_IndexClick = m_RightInput.FindAction("IndexClick", throwIfNotFound: true);
        m_RightInput_IndexTouch = m_RightInput.FindAction("IndexTouch", throwIfNotFound: true);
        m_RightInput_HandTrigger = m_RightInput.FindAction("HandTrigger", throwIfNotFound: true);
        m_RightInput_HandPress = m_RightInput.FindAction("HandPress", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Navigate = m_UI.FindAction("Navigate", throwIfNotFound: true);
        m_UI_Submit = m_UI.FindAction("Submit", throwIfNotFound: true);
        m_UI_Cancel = m_UI.FindAction("Cancel", throwIfNotFound: true);
        m_UI_Point = m_UI.FindAction("Point", throwIfNotFound: true);
        m_UI_Click = m_UI.FindAction("Click", throwIfNotFound: true);
        m_UI_ScrollWheel = m_UI.FindAction("ScrollWheel", throwIfNotFound: true);
        m_UI_MiddleClick = m_UI.FindAction("MiddleClick", throwIfNotFound: true);
        m_UI_RightClick = m_UI.FindAction("RightClick", throwIfNotFound: true);
        m_UI_TrackedDevicePosition = m_UI.FindAction("TrackedDevicePosition", throwIfNotFound: true);
        m_UI_TrackedDeviceOrientation = m_UI.FindAction("TrackedDeviceOrientation", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // LeftInput
    private readonly InputActionMap m_LeftInput;
    private ILeftInputActions m_LeftInputActionsCallbackInterface;
    private readonly InputAction m_LeftInput_StickAxis;
    private readonly InputAction m_LeftInput_StickClick;
    private readonly InputAction m_LeftInput_StickTouch;
    private readonly InputAction m_LeftInput_ThumbTouch;
    private readonly InputAction m_LeftInput_MainPress;
    private readonly InputAction m_LeftInput_MainClick;
    private readonly InputAction m_LeftInput_MainTouch;
    private readonly InputAction m_LeftInput_SubPress;
    private readonly InputAction m_LeftInput_SubClick;
    private readonly InputAction m_LeftInput_SubTouch;
    private readonly InputAction m_LeftInput_IndexTrigger;
    private readonly InputAction m_LeftInput_IndexPress;
    private readonly InputAction m_LeftInput_IndexClick;
    private readonly InputAction m_LeftInput_IndexTouch;
    private readonly InputAction m_LeftInput_HandTrigger;
    private readonly InputAction m_LeftInput_HandPress;
    public struct LeftInputActions : IInputActions
    {
        private @OVRPlayerInput m_Wrapper;
        public LeftInputActions(@OVRPlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @StickAxis => m_Wrapper.m_LeftInput_StickAxis;
        public InputAction @StickClick => m_Wrapper.m_LeftInput_StickClick;
        public InputAction @StickTouch => m_Wrapper.m_LeftInput_StickTouch;
        public InputAction @ThumbTouch => m_Wrapper.m_LeftInput_ThumbTouch;
        public InputAction @MainPress => m_Wrapper.m_LeftInput_MainPress;
        public InputAction @MainClick => m_Wrapper.m_LeftInput_MainClick;
        public InputAction @MainTouch => m_Wrapper.m_LeftInput_MainTouch;
        public InputAction @SubPress => m_Wrapper.m_LeftInput_SubPress;
        public InputAction @SubClick => m_Wrapper.m_LeftInput_SubClick;
        public InputAction @SubTouch => m_Wrapper.m_LeftInput_SubTouch;
        public InputAction @IndexTrigger => m_Wrapper.m_LeftInput_IndexTrigger;
        public InputAction @IndexPress => m_Wrapper.m_LeftInput_IndexPress;
        public InputAction @IndexClick => m_Wrapper.m_LeftInput_IndexClick;
        public InputAction @IndexTouch => m_Wrapper.m_LeftInput_IndexTouch;
        public InputAction @HandTrigger => m_Wrapper.m_LeftInput_HandTrigger;
        public InputAction @HandPress => m_Wrapper.m_LeftInput_HandPress;
        public InputActionMap Get() { return m_Wrapper.m_LeftInput; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(LeftInputActions set) { return set.Get(); }
        public void SetCallbacks(ILeftInputActions instance)
        {
            if (m_Wrapper.m_LeftInputActionsCallbackInterface != null)
            {
                @StickAxis.started -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnStickAxis;
                @StickAxis.performed -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnStickAxis;
                @StickAxis.canceled -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnStickAxis;
                @StickClick.started -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnStickClick;
                @StickClick.performed -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnStickClick;
                @StickClick.canceled -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnStickClick;
                @StickTouch.started -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnStickTouch;
                @StickTouch.performed -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnStickTouch;
                @StickTouch.canceled -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnStickTouch;
                @ThumbTouch.started -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnThumbTouch;
                @ThumbTouch.performed -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnThumbTouch;
                @ThumbTouch.canceled -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnThumbTouch;
                @MainPress.started -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnMainPress;
                @MainPress.performed -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnMainPress;
                @MainPress.canceled -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnMainPress;
                @MainClick.started -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnMainClick;
                @MainClick.performed -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnMainClick;
                @MainClick.canceled -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnMainClick;
                @MainTouch.started -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnMainTouch;
                @MainTouch.performed -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnMainTouch;
                @MainTouch.canceled -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnMainTouch;
                @SubPress.started -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnSubPress;
                @SubPress.performed -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnSubPress;
                @SubPress.canceled -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnSubPress;
                @SubClick.started -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnSubClick;
                @SubClick.performed -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnSubClick;
                @SubClick.canceled -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnSubClick;
                @SubTouch.started -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnSubTouch;
                @SubTouch.performed -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnSubTouch;
                @SubTouch.canceled -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnSubTouch;
                @IndexTrigger.started -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnIndexTrigger;
                @IndexTrigger.performed -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnIndexTrigger;
                @IndexTrigger.canceled -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnIndexTrigger;
                @IndexPress.started -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnIndexPress;
                @IndexPress.performed -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnIndexPress;
                @IndexPress.canceled -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnIndexPress;
                @IndexClick.started -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnIndexClick;
                @IndexClick.performed -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnIndexClick;
                @IndexClick.canceled -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnIndexClick;
                @IndexTouch.started -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnIndexTouch;
                @IndexTouch.performed -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnIndexTouch;
                @IndexTouch.canceled -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnIndexTouch;
                @HandTrigger.started -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnHandTrigger;
                @HandTrigger.performed -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnHandTrigger;
                @HandTrigger.canceled -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnHandTrigger;
                @HandPress.started -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnHandPress;
                @HandPress.performed -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnHandPress;
                @HandPress.canceled -= m_Wrapper.m_LeftInputActionsCallbackInterface.OnHandPress;
            }
            m_Wrapper.m_LeftInputActionsCallbackInterface = instance;
            if (instance != null)
            {
                @StickAxis.started += instance.OnStickAxis;
                @StickAxis.performed += instance.OnStickAxis;
                @StickAxis.canceled += instance.OnStickAxis;
                @StickClick.started += instance.OnStickClick;
                @StickClick.performed += instance.OnStickClick;
                @StickClick.canceled += instance.OnStickClick;
                @StickTouch.started += instance.OnStickTouch;
                @StickTouch.performed += instance.OnStickTouch;
                @StickTouch.canceled += instance.OnStickTouch;
                @ThumbTouch.started += instance.OnThumbTouch;
                @ThumbTouch.performed += instance.OnThumbTouch;
                @ThumbTouch.canceled += instance.OnThumbTouch;
                @MainPress.started += instance.OnMainPress;
                @MainPress.performed += instance.OnMainPress;
                @MainPress.canceled += instance.OnMainPress;
                @MainClick.started += instance.OnMainClick;
                @MainClick.performed += instance.OnMainClick;
                @MainClick.canceled += instance.OnMainClick;
                @MainTouch.started += instance.OnMainTouch;
                @MainTouch.performed += instance.OnMainTouch;
                @MainTouch.canceled += instance.OnMainTouch;
                @SubPress.started += instance.OnSubPress;
                @SubPress.performed += instance.OnSubPress;
                @SubPress.canceled += instance.OnSubPress;
                @SubClick.started += instance.OnSubClick;
                @SubClick.performed += instance.OnSubClick;
                @SubClick.canceled += instance.OnSubClick;
                @SubTouch.started += instance.OnSubTouch;
                @SubTouch.performed += instance.OnSubTouch;
                @SubTouch.canceled += instance.OnSubTouch;
                @IndexTrigger.started += instance.OnIndexTrigger;
                @IndexTrigger.performed += instance.OnIndexTrigger;
                @IndexTrigger.canceled += instance.OnIndexTrigger;
                @IndexPress.started += instance.OnIndexPress;
                @IndexPress.performed += instance.OnIndexPress;
                @IndexPress.canceled += instance.OnIndexPress;
                @IndexClick.started += instance.OnIndexClick;
                @IndexClick.performed += instance.OnIndexClick;
                @IndexClick.canceled += instance.OnIndexClick;
                @IndexTouch.started += instance.OnIndexTouch;
                @IndexTouch.performed += instance.OnIndexTouch;
                @IndexTouch.canceled += instance.OnIndexTouch;
                @HandTrigger.started += instance.OnHandTrigger;
                @HandTrigger.performed += instance.OnHandTrigger;
                @HandTrigger.canceled += instance.OnHandTrigger;
                @HandPress.started += instance.OnHandPress;
                @HandPress.performed += instance.OnHandPress;
                @HandPress.canceled += instance.OnHandPress;
            }
        }
    }
    public LeftInputActions @LeftInput => new LeftInputActions(this);

    // RightInput
    private readonly InputActionMap m_RightInput;
    private IRightInputActions m_RightInputActionsCallbackInterface;
    private readonly InputAction m_RightInput_StickAxis;
    private readonly InputAction m_RightInput_StickClick;
    private readonly InputAction m_RightInput_StickTouch;
    private readonly InputAction m_RightInput_ThumbTouch;
    private readonly InputAction m_RightInput_MainPress;
    private readonly InputAction m_RightInput_MainClick;
    private readonly InputAction m_RightInput_MainTouch;
    private readonly InputAction m_RightInput_SubPress;
    private readonly InputAction m_RightInput_SubClick;
    private readonly InputAction m_RightInput_SubTouch;
    private readonly InputAction m_RightInput_IndexTrigger;
    private readonly InputAction m_RightInput_IndexPress;
    private readonly InputAction m_RightInput_IndexClick;
    private readonly InputAction m_RightInput_IndexTouch;
    private readonly InputAction m_RightInput_HandTrigger;
    private readonly InputAction m_RightInput_HandPress;
    public struct RightInputActions : IInputActions
    {
        private @OVRPlayerInput m_Wrapper;
        public RightInputActions(@OVRPlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @StickAxis => m_Wrapper.m_RightInput_StickAxis;
        public InputAction @StickClick => m_Wrapper.m_RightInput_StickClick;
        public InputAction @StickTouch => m_Wrapper.m_RightInput_StickTouch;
        public InputAction @ThumbTouch => m_Wrapper.m_RightInput_ThumbTouch;
        public InputAction @MainPress => m_Wrapper.m_RightInput_MainPress;
        public InputAction @MainClick => m_Wrapper.m_RightInput_MainClick;
        public InputAction @MainTouch => m_Wrapper.m_RightInput_MainTouch;
        public InputAction @SubPress => m_Wrapper.m_RightInput_SubPress;
        public InputAction @SubClick => m_Wrapper.m_RightInput_SubClick;
        public InputAction @SubTouch => m_Wrapper.m_RightInput_SubTouch;
        public InputAction @IndexTrigger => m_Wrapper.m_RightInput_IndexTrigger;
        public InputAction @IndexPress => m_Wrapper.m_RightInput_IndexPress;
        public InputAction @IndexClick => m_Wrapper.m_RightInput_IndexClick;
        public InputAction @IndexTouch => m_Wrapper.m_RightInput_IndexTouch;
        public InputAction @HandTrigger => m_Wrapper.m_RightInput_HandTrigger;
        public InputAction @HandPress => m_Wrapper.m_RightInput_HandPress;
        public InputActionMap Get() { return m_Wrapper.m_RightInput; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(RightInputActions set) { return set.Get(); }
        public void SetCallbacks(IRightInputActions instance)
        {
            if (m_Wrapper.m_RightInputActionsCallbackInterface != null)
            {
                @StickAxis.started -= m_Wrapper.m_RightInputActionsCallbackInterface.OnStickAxis;
                @StickAxis.performed -= m_Wrapper.m_RightInputActionsCallbackInterface.OnStickAxis;
                @StickAxis.canceled -= m_Wrapper.m_RightInputActionsCallbackInterface.OnStickAxis;
                @StickClick.started -= m_Wrapper.m_RightInputActionsCallbackInterface.OnStickClick;
                @StickClick.performed -= m_Wrapper.m_RightInputActionsCallbackInterface.OnStickClick;
                @StickClick.canceled -= m_Wrapper.m_RightInputActionsCallbackInterface.OnStickClick;
                @StickTouch.started -= m_Wrapper.m_RightInputActionsCallbackInterface.OnStickTouch;
                @StickTouch.performed -= m_Wrapper.m_RightInputActionsCallbackInterface.OnStickTouch;
                @StickTouch.canceled -= m_Wrapper.m_RightInputActionsCallbackInterface.OnStickTouch;
                @ThumbTouch.started -= m_Wrapper.m_RightInputActionsCallbackInterface.OnThumbTouch;
                @ThumbTouch.performed -= m_Wrapper.m_RightInputActionsCallbackInterface.OnThumbTouch;
                @ThumbTouch.canceled -= m_Wrapper.m_RightInputActionsCallbackInterface.OnThumbTouch;
                @MainPress.started -= m_Wrapper.m_RightInputActionsCallbackInterface.OnMainPress;
                @MainPress.performed -= m_Wrapper.m_RightInputActionsCallbackInterface.OnMainPress;
                @MainPress.canceled -= m_Wrapper.m_RightInputActionsCallbackInterface.OnMainPress;
                @MainClick.started -= m_Wrapper.m_RightInputActionsCallbackInterface.OnMainClick;
                @MainClick.performed -= m_Wrapper.m_RightInputActionsCallbackInterface.OnMainClick;
                @MainClick.canceled -= m_Wrapper.m_RightInputActionsCallbackInterface.OnMainClick;
                @MainTouch.started -= m_Wrapper.m_RightInputActionsCallbackInterface.OnMainTouch;
                @MainTouch.performed -= m_Wrapper.m_RightInputActionsCallbackInterface.OnMainTouch;
                @MainTouch.canceled -= m_Wrapper.m_RightInputActionsCallbackInterface.OnMainTouch;
                @SubPress.started -= m_Wrapper.m_RightInputActionsCallbackInterface.OnSubPress;
                @SubPress.performed -= m_Wrapper.m_RightInputActionsCallbackInterface.OnSubPress;
                @SubPress.canceled -= m_Wrapper.m_RightInputActionsCallbackInterface.OnSubPress;
                @SubClick.started -= m_Wrapper.m_RightInputActionsCallbackInterface.OnSubClick;
                @SubClick.performed -= m_Wrapper.m_RightInputActionsCallbackInterface.OnSubClick;
                @SubClick.canceled -= m_Wrapper.m_RightInputActionsCallbackInterface.OnSubClick;
                @SubTouch.started -= m_Wrapper.m_RightInputActionsCallbackInterface.OnSubTouch;
                @SubTouch.performed -= m_Wrapper.m_RightInputActionsCallbackInterface.OnSubTouch;
                @SubTouch.canceled -= m_Wrapper.m_RightInputActionsCallbackInterface.OnSubTouch;
                @IndexTrigger.started -= m_Wrapper.m_RightInputActionsCallbackInterface.OnIndexTrigger;
                @IndexTrigger.performed -= m_Wrapper.m_RightInputActionsCallbackInterface.OnIndexTrigger;
                @IndexTrigger.canceled -= m_Wrapper.m_RightInputActionsCallbackInterface.OnIndexTrigger;
                @IndexPress.started -= m_Wrapper.m_RightInputActionsCallbackInterface.OnIndexPress;
                @IndexPress.performed -= m_Wrapper.m_RightInputActionsCallbackInterface.OnIndexPress;
                @IndexPress.canceled -= m_Wrapper.m_RightInputActionsCallbackInterface.OnIndexPress;
                @IndexClick.started -= m_Wrapper.m_RightInputActionsCallbackInterface.OnIndexClick;
                @IndexClick.performed -= m_Wrapper.m_RightInputActionsCallbackInterface.OnIndexClick;
                @IndexClick.canceled -= m_Wrapper.m_RightInputActionsCallbackInterface.OnIndexClick;
                @IndexTouch.started -= m_Wrapper.m_RightInputActionsCallbackInterface.OnIndexTouch;
                @IndexTouch.performed -= m_Wrapper.m_RightInputActionsCallbackInterface.OnIndexTouch;
                @IndexTouch.canceled -= m_Wrapper.m_RightInputActionsCallbackInterface.OnIndexTouch;
                @HandTrigger.started -= m_Wrapper.m_RightInputActionsCallbackInterface.OnHandTrigger;
                @HandTrigger.performed -= m_Wrapper.m_RightInputActionsCallbackInterface.OnHandTrigger;
                @HandTrigger.canceled -= m_Wrapper.m_RightInputActionsCallbackInterface.OnHandTrigger;
                @HandPress.started -= m_Wrapper.m_RightInputActionsCallbackInterface.OnHandPress;
                @HandPress.performed -= m_Wrapper.m_RightInputActionsCallbackInterface.OnHandPress;
                @HandPress.canceled -= m_Wrapper.m_RightInputActionsCallbackInterface.OnHandPress;
            }
            m_Wrapper.m_RightInputActionsCallbackInterface = instance;
            if (instance != null)
            {
                @StickAxis.started += instance.OnStickAxis;
                @StickAxis.performed += instance.OnStickAxis;
                @StickAxis.canceled += instance.OnStickAxis;
                @StickClick.started += instance.OnStickClick;
                @StickClick.performed += instance.OnStickClick;
                @StickClick.canceled += instance.OnStickClick;
                @StickTouch.started += instance.OnStickTouch;
                @StickTouch.performed += instance.OnStickTouch;
                @StickTouch.canceled += instance.OnStickTouch;
                @ThumbTouch.started += instance.OnThumbTouch;
                @ThumbTouch.performed += instance.OnThumbTouch;
                @ThumbTouch.canceled += instance.OnThumbTouch;
                @MainPress.started += instance.OnMainPress;
                @MainPress.performed += instance.OnMainPress;
                @MainPress.canceled += instance.OnMainPress;
                @MainClick.started += instance.OnMainClick;
                @MainClick.performed += instance.OnMainClick;
                @MainClick.canceled += instance.OnMainClick;
                @MainTouch.started += instance.OnMainTouch;
                @MainTouch.performed += instance.OnMainTouch;
                @MainTouch.canceled += instance.OnMainTouch;
                @SubPress.started += instance.OnSubPress;
                @SubPress.performed += instance.OnSubPress;
                @SubPress.canceled += instance.OnSubPress;
                @SubClick.started += instance.OnSubClick;
                @SubClick.performed += instance.OnSubClick;
                @SubClick.canceled += instance.OnSubClick;
                @SubTouch.started += instance.OnSubTouch;
                @SubTouch.performed += instance.OnSubTouch;
                @SubTouch.canceled += instance.OnSubTouch;
                @IndexTrigger.started += instance.OnIndexTrigger;
                @IndexTrigger.performed += instance.OnIndexTrigger;
                @IndexTrigger.canceled += instance.OnIndexTrigger;
                @IndexPress.started += instance.OnIndexPress;
                @IndexPress.performed += instance.OnIndexPress;
                @IndexPress.canceled += instance.OnIndexPress;
                @IndexClick.started += instance.OnIndexClick;
                @IndexClick.performed += instance.OnIndexClick;
                @IndexClick.canceled += instance.OnIndexClick;
                @IndexTouch.started += instance.OnIndexTouch;
                @IndexTouch.performed += instance.OnIndexTouch;
                @IndexTouch.canceled += instance.OnIndexTouch;
                @HandTrigger.started += instance.OnHandTrigger;
                @HandTrigger.performed += instance.OnHandTrigger;
                @HandTrigger.canceled += instance.OnHandTrigger;
                @HandPress.started += instance.OnHandPress;
                @HandPress.performed += instance.OnHandPress;
                @HandPress.canceled += instance.OnHandPress;
            }
        }
    }
    public RightInputActions @RightInput => new RightInputActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Navigate;
    private readonly InputAction m_UI_Submit;
    private readonly InputAction m_UI_Cancel;
    private readonly InputAction m_UI_Point;
    private readonly InputAction m_UI_Click;
    private readonly InputAction m_UI_ScrollWheel;
    private readonly InputAction m_UI_MiddleClick;
    private readonly InputAction m_UI_RightClick;
    private readonly InputAction m_UI_TrackedDevicePosition;
    private readonly InputAction m_UI_TrackedDeviceOrientation;
    public struct UIActions
    {
        private @OVRPlayerInput m_Wrapper;
        public UIActions(@OVRPlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Navigate => m_Wrapper.m_UI_Navigate;
        public InputAction @Submit => m_Wrapper.m_UI_Submit;
        public InputAction @Cancel => m_Wrapper.m_UI_Cancel;
        public InputAction @Point => m_Wrapper.m_UI_Point;
        public InputAction @Click => m_Wrapper.m_UI_Click;
        public InputAction @ScrollWheel => m_Wrapper.m_UI_ScrollWheel;
        public InputAction @MiddleClick => m_Wrapper.m_UI_MiddleClick;
        public InputAction @RightClick => m_Wrapper.m_UI_RightClick;
        public InputAction @TrackedDevicePosition => m_Wrapper.m_UI_TrackedDevicePosition;
        public InputAction @TrackedDeviceOrientation => m_Wrapper.m_UI_TrackedDeviceOrientation;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Navigate.started -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Submit.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Cancel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Point.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Point.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Point.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Click.started -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @ScrollWheel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @MiddleClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @RightClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @TrackedDevicePosition.started -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDevicePosition.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDevicePosition.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDevicePosition;
                @TrackedDeviceOrientation.started -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnTrackedDeviceOrientation;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Point.started += instance.OnPoint;
                @Point.performed += instance.OnPoint;
                @Point.canceled += instance.OnPoint;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @ScrollWheel.started += instance.OnScrollWheel;
                @ScrollWheel.performed += instance.OnScrollWheel;
                @ScrollWheel.canceled += instance.OnScrollWheel;
                @MiddleClick.started += instance.OnMiddleClick;
                @MiddleClick.performed += instance.OnMiddleClick;
                @MiddleClick.canceled += instance.OnMiddleClick;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
                @TrackedDevicePosition.started += instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.performed += instance.OnTrackedDevicePosition;
                @TrackedDevicePosition.canceled += instance.OnTrackedDevicePosition;
                @TrackedDeviceOrientation.started += instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.performed += instance.OnTrackedDeviceOrientation;
                @TrackedDeviceOrientation.canceled += instance.OnTrackedDeviceOrientation;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_TouchSchemeIndex = -1;
    public InputControlScheme TouchScheme
    {
        get
        {
            if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
            return asset.controlSchemes[m_TouchSchemeIndex];
        }
    }
    private int m_JoystickSchemeIndex = -1;
    public InputControlScheme JoystickScheme
    {
        get
        {
            if (m_JoystickSchemeIndex == -1) m_JoystickSchemeIndex = asset.FindControlSchemeIndex("Joystick");
            return asset.controlSchemes[m_JoystickSchemeIndex];
        }
    }
    private int m_XRSchemeIndex = -1;
    public InputControlScheme XRScheme
    {
        get
        {
            if (m_XRSchemeIndex == -1) m_XRSchemeIndex = asset.FindControlSchemeIndex("XR");
            return asset.controlSchemes[m_XRSchemeIndex];
        }
    }
    public interface ILeftInputActions
    {
        void OnStickAxis(InputAction.CallbackContext context);
        void OnStickClick(InputAction.CallbackContext context);
        void OnStickTouch(InputAction.CallbackContext context);
        void OnThumbTouch(InputAction.CallbackContext context);
        void OnMainPress(InputAction.CallbackContext context);
        void OnMainClick(InputAction.CallbackContext context);
        void OnMainTouch(InputAction.CallbackContext context);
        void OnSubPress(InputAction.CallbackContext context);
        void OnSubClick(InputAction.CallbackContext context);
        void OnSubTouch(InputAction.CallbackContext context);
        void OnIndexTrigger(InputAction.CallbackContext context);
        void OnIndexPress(InputAction.CallbackContext context);
        void OnIndexClick(InputAction.CallbackContext context);
        void OnIndexTouch(InputAction.CallbackContext context);
        void OnHandTrigger(InputAction.CallbackContext context);
        void OnHandPress(InputAction.CallbackContext context);
    }
    public interface IRightInputActions
    {
        void OnStickAxis(InputAction.CallbackContext context);
        void OnStickClick(InputAction.CallbackContext context);
        void OnStickTouch(InputAction.CallbackContext context);
        void OnThumbTouch(InputAction.CallbackContext context);
        void OnMainPress(InputAction.CallbackContext context);
        void OnMainClick(InputAction.CallbackContext context);
        void OnMainTouch(InputAction.CallbackContext context);
        void OnSubPress(InputAction.CallbackContext context);
        void OnSubClick(InputAction.CallbackContext context);
        void OnSubTouch(InputAction.CallbackContext context);
        void OnIndexTrigger(InputAction.CallbackContext context);
        void OnIndexPress(InputAction.CallbackContext context);
        void OnIndexClick(InputAction.CallbackContext context);
        void OnIndexTouch(InputAction.CallbackContext context);
        void OnHandTrigger(InputAction.CallbackContext context);
        void OnHandPress(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnNavigate(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnPoint(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnScrollWheel(InputAction.CallbackContext context);
        void OnMiddleClick(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnTrackedDevicePosition(InputAction.CallbackContext context);
        void OnTrackedDeviceOrientation(InputAction.CallbackContext context);
    }

    public interface IInputActions
    {
        InputAction StickAxis { get; }
        InputAction StickClick { get; }
        InputAction StickTouch { get; }
        InputAction ThumbTouch { get; }
        InputAction MainPress { get; }
        InputAction MainClick { get; }
        InputAction MainTouch { get; }
        InputAction SubPress { get; }
        InputAction SubClick { get; }
        InputAction SubTouch { get; }
        InputAction IndexTrigger { get; }
        InputAction IndexPress { get; }
        InputAction IndexClick { get; }
        InputAction IndexTouch { get; }
        InputAction HandTrigger { get; }
        InputAction HandPress { get; }
    }
}
