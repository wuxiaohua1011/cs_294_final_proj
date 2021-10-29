// GENERATED AUTOMATICALLY FROM 'Assets/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Controls"",
            ""id"": ""a50917de-388d-497e-aa41-e91c42969b27"",
            ""actions"": [
                {
                    ""name"": ""throttle"",
                    ""type"": ""Value"",
                    ""id"": ""30a4ea8b-d7c1-43d0-aec5-43ec85b6e369"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""steering"",
                    ""type"": ""Value"",
                    ""id"": ""b35218fe-fa9a-4390-b2be-822f7e6eff6f"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""88e76623-39d8-4e2d-91c5-0279906b157c"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""throttle"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""aad2b4cf-c16c-4fe6-a0a0-8ffe6235950e"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""throttle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""2637bc52-3679-4775-947d-6774e4b5fb6e"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""throttle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5ef7a201-45bf-454d-becd-b58031cfe5e6"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""steering"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Controls
        m_Controls = asset.FindActionMap("Controls", throwIfNotFound: true);
        m_Controls_throttle = m_Controls.FindAction("throttle", throwIfNotFound: true);
        m_Controls_steering = m_Controls.FindAction("steering", throwIfNotFound: true);
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

    // Controls
    private readonly InputActionMap m_Controls;
    private IControlsActions m_ControlsActionsCallbackInterface;
    private readonly InputAction m_Controls_throttle;
    private readonly InputAction m_Controls_steering;
    public struct ControlsActions
    {
        private @PlayerControls m_Wrapper;
        public ControlsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @throttle => m_Wrapper.m_Controls_throttle;
        public InputAction @steering => m_Wrapper.m_Controls_steering;
        public InputActionMap Get() { return m_Wrapper.m_Controls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ControlsActions set) { return set.Get(); }
        public void SetCallbacks(IControlsActions instance)
        {
            if (m_Wrapper.m_ControlsActionsCallbackInterface != null)
            {
                @throttle.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnThrottle;
                @throttle.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnThrottle;
                @throttle.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnThrottle;
                @steering.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnSteering;
                @steering.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnSteering;
                @steering.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnSteering;
            }
            m_Wrapper.m_ControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @throttle.started += instance.OnThrottle;
                @throttle.performed += instance.OnThrottle;
                @throttle.canceled += instance.OnThrottle;
                @steering.started += instance.OnSteering;
                @steering.performed += instance.OnSteering;
                @steering.canceled += instance.OnSteering;
            }
        }
    }
    public ControlsActions @Controls => new ControlsActions(this);
    public interface IControlsActions
    {
        void OnThrottle(InputAction.CallbackContext context);
        void OnSteering(InputAction.CallbackContext context);
    }
}
