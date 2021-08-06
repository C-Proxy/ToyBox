using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputManager : SingletonBehaviour<InputManager>
{
    OVRPlayerInput m_PlayerInput;
    public static HandInput CreateHandInput(InputType type) => type switch
    {
        InputType.LeftController => new HandInput(_Singleton.m_PlayerInput.LeftInput),
        InputType.RightController => new HandInput(_Singleton.m_PlayerInput.RightInput),
        _ => null
    };

    IControllable m_Controllable;

    override protected void Awake()
    {
        base.Awake();
        m_PlayerInput = new OVRPlayerInput();
        m_PlayerInput.Enable();
    }

    public class HandInput
    {
        readonly OVRPlayerInput.IInputActions m_InputActions;
        readonly public UnityEvent<Vector2> StickAxis = new UnityEvent<Vector2>();
        readonly public UnityEvent<bool> StickClick = new UnityEvent<bool>();
        readonly public UnityEvent<bool> StickTouch = new UnityEvent<bool>();

        readonly public UnityEvent<bool> MainClick = new UnityEvent<bool>();
        readonly public UnityEvent<bool> MainPress = new UnityEvent<bool>();
        readonly public UnityEvent<bool> MainTouch = new UnityEvent<bool>();

        readonly public UnityEvent<bool> SubClick = new UnityEvent<bool>();
        readonly public UnityEvent<bool> SubPress = new UnityEvent<bool>();
        readonly public UnityEvent<bool> SubTouch = new UnityEvent<bool>();

        readonly public UnityEvent<float> IndexTrigger = new UnityEvent<float>();
        readonly public UnityEvent<bool> IndexPress = new UnityEvent<bool>();
        readonly public UnityEvent<bool> IndexClick = new UnityEvent<bool>();
        readonly public UnityEvent<bool> IndexTouch = new UnityEvent<bool>();

        readonly public UnityEvent<float> HandTrigger = new UnityEvent<float>();
        readonly public UnityEvent<bool> HandPress = new UnityEvent<bool>();

        void InvokeStickAxis(InputAction.CallbackContext context) => StickAxis.Invoke(context.ReadValue<Vector2>());
        void InvokeStickSingleClick(InputAction.CallbackContext context) => StickClick.Invoke(false);
        void InvokeStickDoubleClick(InputAction.CallbackContext context) => StickClick.Invoke(true);
        void InvokeStickTouch(InputAction.CallbackContext context) => StickTouch.Invoke(context.ReadValueAsButton());

        void InvokeMainSingleClick(InputAction.CallbackContext context) => MainClick.Invoke(false);
        void InvokeMainDoubleClick(InputAction.CallbackContext context) => MainClick.Invoke(true);
        void InvokeMainPress(InputAction.CallbackContext context) => MainPress.Invoke(context.ReadValueAsButton());
        void InvokeMainTouch(InputAction.CallbackContext context) => MainTouch.Invoke(context.ReadValueAsButton());

        void InvokeSubSingleClick(InputAction.CallbackContext context) => SubClick.Invoke(false);
        void InvokeSubDoubleClick(InputAction.CallbackContext context) => SubClick.Invoke(true);
        void InvokeSubPress(InputAction.CallbackContext context) => SubPress.Invoke(context.ReadValueAsButton());
        void InvokeSubTouch(InputAction.CallbackContext context) => SubTouch.Invoke(context.ReadValueAsButton());

        void InvokeIndexSingleClick(InputAction.CallbackContext context) => IndexClick.Invoke(false);
        void InvokeIndexDoubleClick(InputAction.CallbackContext context) => IndexClick.Invoke(true);
        void InvokeIndexPress(InputAction.CallbackContext context) => IndexPress.Invoke(context.ReadValueAsButton());
        void InvokeIndexTrigger(InputAction.CallbackContext context) => IndexTrigger.Invoke(context.ReadValue<float>());
        void InvokeIndexTouch(InputAction.CallbackContext context) => IndexTouch.Invoke(context.ReadValueAsButton());

        void InvokeHandTrigger(InputAction.CallbackContext context) => HandTrigger.Invoke(context.ReadValue<float>());
        void InvokeHandPress(InputAction.CallbackContext context) => HandPress.Invoke(context.ReadValueAsButton());

        public HandInput(OVRPlayerInput.IInputActions input)
        {
            m_InputActions = input;

            input.StickAxis.performed += InvokeStickAxis;
            input.StickClick.performed += InvokeStickSingleClick;
            input.StickClick.canceled += InvokeStickDoubleClick;
            input.StickTouch.performed += InvokeStickTouch;

            input.MainClick.performed += InvokeMainDoubleClick;
            input.MainClick.canceled += InvokeMainSingleClick;
            input.MainPress.performed += InvokeMainPress;
            input.MainTouch.performed += InvokeMainTouch;

            input.SubClick.performed += InvokeSubDoubleClick;
            input.SubClick.canceled += InvokeSubSingleClick;
            input.SubPress.performed += InvokeSubPress;
            input.SubTouch.performed += InvokeSubTouch;

            input.IndexTrigger.performed += InvokeIndexTrigger;
            input.IndexClick.performed += InvokeIndexDoubleClick;
            input.IndexClick.canceled += InvokeIndexSingleClick;
            input.IndexPress.performed += InvokeIndexPress;
            input.IndexTouch.performed += InvokeIndexTouch;

            input.HandTrigger.performed += InvokeHandTrigger;
            input.HandPress.performed += InvokeHandPress;
        }
        public void Destroy()
        {
            var input = m_InputActions;

            input.StickAxis.performed -= InvokeStickAxis;
            input.StickClick.performed -= InvokeStickSingleClick;
            input.StickClick.canceled -= InvokeStickDoubleClick;
            input.StickTouch.performed -= InvokeStickTouch;

            input.MainClick.performed -= InvokeMainDoubleClick;
            input.MainClick.canceled -= InvokeMainSingleClick;
            input.MainPress.performed -= InvokeMainPress;
            input.MainTouch.performed -= InvokeMainTouch;

            input.SubClick.performed -= InvokeSubDoubleClick;
            input.SubClick.canceled -= InvokeSubSingleClick;
            input.SubPress.performed -= InvokeSubPress;
            input.SubTouch.performed -= InvokeSubTouch;

            input.IndexTrigger.performed -= InvokeIndexTrigger;
            input.IndexClick.performed -= InvokeIndexDoubleClick;
            input.IndexClick.canceled -= InvokeIndexSingleClick;
            input.IndexPress.performed -= InvokeIndexPress;
            input.IndexTouch.performed -= InvokeIndexTouch;

            input.HandTrigger.performed -= InvokeHandTrigger;
            input.HandPress.performed -= InvokeHandPress;

            StickAxis.RemoveAllListeners();
            StickClick.RemoveAllListeners();
            StickTouch.RemoveAllListeners();

            MainClick.RemoveAllListeners();
            MainPress.RemoveAllListeners();
            MainTouch.RemoveAllListeners();

            SubClick.RemoveAllListeners();
            SubPress.RemoveAllListeners();
            SubTouch.RemoveAllListeners();

            IndexTrigger.RemoveAllListeners();
            IndexClick.RemoveAllListeners();
            IndexPress.RemoveAllListeners();
            IndexTouch.RemoveAllListeners();

            HandTrigger.RemoveAllListeners();
            HandPress.RemoveAllListeners();
        }
    }
    public enum InputType
    {
        LeftController,
        RightController,
    }
}
