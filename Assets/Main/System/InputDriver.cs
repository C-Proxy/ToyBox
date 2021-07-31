using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using UniRx.Triggers;

public class InputDriver : SingletonBehaviour<InputDriver>
{
    PlayerInput m_LeftInput, m_RightInput;
    public static PlayerInput GetPlayerInput(bool isLeft)
    {
        if (isLeft)
            return _Singleton.m_LeftInput;
        else
            return _Singleton.m_RightInput;
    }
    // ReactiveProperty<IControllable> m_LeftControlRP = new ReactiveProperty<IControllable>();
    // ReactiveProperty<IControllable> m_RightControlRP = new ReactiveProperty<IControllable>();
    // IControllable LeftControl { set { m_LeftControlRP.Value = value; } get { return m_LeftControlRP.Value; } }
    // IControllable RightControl { set { m_RightControlRP.Value = value; } get { return m_RightControlRP.Value; } }
    /*
    public static IObservable<Unit> InputAsObservable(bool isLeft,InputKey key, InputType type)
    {
        var tuple = (isLeft,key, type);
        var dic = _Singleton.m_UnitDictionary;
        if (dic.TryGetValue(tuple, out var value))
            return value;
        else
        {
            var observable = CreationInputAsObservable(_Singleton.m_UpdateAsObservable,isLeft, key, type);
            dic.Add(tuple, observable);
            return observable;
        }
    }
    public static IObservable<bool> ClickAsObservable(bool isLeft,InputKey key, InputType type)
    {
        var tuple = (isLeft,key, type);
        var dic = _Singleton.m_ClickDictionary;
        if (dic.TryGetValue(tuple, out var value))
            return value;
        else
        {
            var observable = CreationClickAsObservable(InputAsObservable(isLeft,key, type));
            dic.Add(tuple, observable);
            return observable;
        }
    }
    public static IObservable<float> InputAxis1DAsObservable(bool isLeft,InputKey key)
    {
        var tuple = (isLeft, key);
        var dic = _Singleton.m_Axis1DDictionary;
        if (dic.TryGetValue(tuple, out var value))
            return value;
        else
        {
            var observable = CreationInputAxis1DAsObservable(_Singleton.m_UpdateAsObservable, key);
            dic.Add(tuple, observable);
            return observable;
        }
    }
    public static IObservable<Vector2> InputAxis2DAsObservable(bool isLeft,InputKey key)
    {
        var tuple = (isLeft, key);
        var dic = _Singleton.m_Axis2DDicitionary;
        if (dic.TryGetValue(tuple, out var value))
            return value;
        else
        {
            var observable = CreationInputAxis2DAsObservable(_Singleton.m_UpdateAsObservable, key);
            dic.Add(tuple, observable);
            return observable;
        }
    }

    static IObservable<Unit> CreationInputAsObservable(IObservable<Unit> source,bool isLeft, InputKey key, InputType type)
    {
        switch (type)
        {
            case InputType.ButtonDown:
                return source.Where(_ => OVRInput.GetDown(ConvertToRawButton(isLeft,key))).Publish().RefCount();
            case InputType.ButtonUp:
                return source.Where(_ => OVRInput.GetUp(ConvertToRawButton(isLeft,key))).Publish().RefCount();
            case InputType.TouchDown:
                return source.Where(_ => OVRInput.GetDown(ConvertToRawTouch(isLeft,key))).Publish().RefCount();
            case InputType.TouchUp:
                return source.Where(_ => OVRInput.GetUp(ConvertToRawTouch(isLeft,key))).Publish().RefCount();
            default:
                Debug.LogWarning($"{type} is Unavailable in CreationInputAsObservable.");
                return null;
        }
    }
    static IObservable<bool> CreationClickAsObservable(IObservable<Unit> source)
    => source.Buffer(source.Throttle(TimeSpan.FromMilliseconds(DOUBLECLICK_INTERVAL))).Select(buf => buf.Count >= 2).Publish().RefCount();
    static IObservable<float> CreationInputAxis1DAsObservable(IObservable<Unit> source, InputKey key)
    => source.Select(_ => OVRInput.Get(ConvertToRawAxis1D(key))).Publish().RefCount();
    static IObservable<Vector2> CreationInputAxis2DAsObservable(IObservable<Unit> source, InputKey key)
    => source.Select(_ => OVRInput.Get(ConvertToRawAxis2D(key))).Publish().RefCount();
    static OVRInput.RawButton ConvertToRawButton(bool isLeft, KeyType key)
    {
        switch (key)
        {
            case KeyType.MainButton:
                return isLeft ? OVRInput.RawButton.A : OVRInput.RawButton.X;
            case KeyType.SubButton:
                return isLeft ? OVRInput.RawButton.B : OVRInput.RawButton.Y;
            case KeyType.Stick:
                return isLeft ? OVRInput.RawButton.LThumbstick : OVRInput.RawButton.RThumbstick;
            case KeyType.IndexTrigger:
                return isLeft ? OVRInput.RawButton.LIndexTrigger : OVRInput.RawButton.RIndexTrigger;
            case KeyType.HandTrigger:
                return isLeft ? OVRInput.RawButton.LHandTrigger : OVRInput.RawButton.RHandTrigger;
        }
        Debug.Log($"{key} is Not Assigned in RawButton");
        return default;
    }
    static OVRInput.RawTouch ConvertToRawTouch(bool isLeft, KeyType key)
    {
        switch (key)
        {
            case KeyType.MainButton:
                return isLeft ? OVRInput.RawTouch.A : OVRInput.RawTouch.X;
            case KeyType.SubButton:
                return isLeft ? OVRInput.RawTouch.B : OVRInput.RawTouch.Y;
            case KeyType.Stick:
                return isLeft ? OVRInput.RawTouch.LThumbstick : OVRInput.RawTouch.RThumbstick;
            case KeyType.IndexTrigger:
                return isLeft ? OVRInput.RawTouch.LIndexTrigger : OVRInput.RawTouch.RIndexTrigger;
        }
        Debug.Log($"{key} is Not Assigned in RawTouch");
        return default;
    }
    static OVRInput.RawAxis1D ConvertToRawAxis1D(bool isLeft, KeyType key)
    {
        switch (key)
        {
            case KeyType.IndexTrigger:
                return isLeft ? OVRInput.RawAxis1D.LIndexTrigger : OVRInput.RawAxis1D.RIndexTrigger;
            case KeyType.HandTrigger:
                return isLeft ? OVRInput.RawAxis1D.LHandTrigger : OVRInput.RawAxis1D.RHandTrigger;
        }
        Debug.Log($"{key} is Not Assigned in RawAxis1D");
        return default;
    }
    static OVRInput.RawAxis2D ConvertToRawAxis2D(bool isLeft, KeyType key)
    {
        switch (key)
        {
            case KeyType.Stick:
                return isLeft ? OVRInput.RawAxis2D.LThumbstick : OVRInput.RawAxis2D.RThumbstick;
        }
        Debug.Log($"{key} is Not Assingned in RawAxis2D");
        return default;
    }
    */
    override protected void Awake()
    {
        base.Awake();
        var buttons = new[]{
            KeyType.MainButton,
            KeyType.SubButton,
            KeyType.Stick,
            KeyType.IndexTrigger,
            KeyType.HandTrigger
        };
        var touches = new[]{
            KeyType.MainButton,
            KeyType.SubButton,
            KeyType.Stick,
            KeyType.IndexTrigger,
        };
        var axis1Ds = new[]{
            KeyType.IndexTrigger,
            KeyType.HandTrigger
        };
        var axis2Ds = new[]{
            KeyType.Stick
        };
        m_LeftInput = PlayerInput.CreationFromKeys(
            buttons.Zip(
                new[] {
                OVRInput.RawButton.X,
                OVRInput.RawButton.Y,
                OVRInput.RawButton.LThumbstick,
                OVRInput.RawButton.LIndexTrigger,
                OVRInput.RawButton.LHandTrigger
            }, (type, key) => (type, key)).ToArray(),
            touches.Zip(
                new[] {
                OVRInput.RawTouch.X,
                OVRInput.RawTouch.Y,
                OVRInput.RawTouch.LThumbstick,
                OVRInput.RawTouch.LIndexTrigger
            }, (type, key) => (type, key)).ToArray(),
            axis1Ds.Zip(
                new[] {
                OVRInput.RawAxis1D.LIndexTrigger,
                OVRInput.RawAxis1D.LHandTrigger
            }, (type, key) => (type, key)).ToArray(),
            axis2Ds.Zip(
                new[] {
                OVRInput.RawAxis2D.LThumbstick
            }, (type, key) => (type, key)).ToArray()
        );
        m_RightInput = PlayerInput.CreationFromKeys(
            buttons.Zip(
                new[] {
                OVRInput.RawButton.A,
                OVRInput.RawButton.B,
                OVRInput.RawButton.RThumbstick,
                OVRInput.RawButton.RIndexTrigger,
                OVRInput.RawButton.RHandTrigger
            }, (type, key) => (type, key)).ToArray(),
            touches.Zip(
                new[] {
                OVRInput.RawTouch.A,
                OVRInput.RawTouch.B,
                OVRInput.RawTouch.RThumbstick,
                OVRInput.RawTouch.RIndexTrigger
            }, (type, key) => (type, key)).ToArray(),
            axis1Ds.Zip(
                new[] {
                OVRInput.RawAxis1D.RIndexTrigger,
                OVRInput.RawAxis1D.RHandTrigger
            }, (type, key) => (type, key)).ToArray(),
            axis2Ds.Zip(
                new[] {
                OVRInput.RawAxis2D.RThumbstick
            }, (type, key) => (type, key)).ToArray()
        );
        // IDisposable[] leftDiscriptions = default, rightDiscriptions = default;
        // m_LeftControlRP.Subscribe(controllable =>
        // {
        //     if (leftDiscriptions != null)
        //         foreach (var disposable in leftDiscriptions)
        //             disposable.Dispose();
        //     if (controllable != null)
        //         leftDiscriptions = controllable.Connect(m_LeftInput);
        // }).AddTo(this);
        // m_RightControlRP.Subscribe(controllable =>
        // {
        //     if (rightDiscriptions != null)
        //         foreach (var disposable in rightDiscriptions)
        //             disposable.Dispose();
        //     if (controllable != null)
        //         rightDiscriptions = controllable.Connect(m_RightInput);
        // }).AddTo(this);
    }
    private void Update()
    {
        m_LeftInput.Send();
        m_RightInput.Send();
    }
    // public static void SetControl(bool isLeft, IControllable controllable)
    // {
    //     if (isLeft)
    //         _Singleton.LeftControl = controllable;
    //     else
    //         _Singleton.RightControl = controllable;
    // }
    public struct PlayerInput
    {
        const float DOUBLECLICK_INTERVAL = 200f;
        Dictionary<KeyType, (OVRInput.RawButton RawButton, Subject<bool> Subject)> m_KeepPressSubjectDictionary;
        Dictionary<KeyType, (OVRInput.RawTouch RawTouch, Subject<bool> Subject)> m_KeepTouchSubjectDictionary;
        Dictionary<KeyType, (OVRInput.RawAxis1D RawAxis1D, Subject<float> Subject)> m_Axis1DSubjectDictionary;
        Dictionary<KeyType, (OVRInput.RawAxis2D RawAxis2D, Subject<Vector2> Subject)> m_Axis2DSubjectDictionary;
        Dictionary<KeyType, IObservable<bool>> m_ButtonAsObservableDictionary;
        Dictionary<KeyType, IObservable<bool>> m_TouchAsObservableDictionary;
        Dictionary<HandInput, IObservable<Unit>> m_HandInputAsObservableDictionary;
        IObservable<bool> m_PickAsObservable;
        Dictionary<KeyType, IObservable<bool>> m_ClickDictionary;

        public PlayerInput(
            Dictionary<KeyType, (OVRInput.RawButton Button, Subject<bool> Subject)> keepPressDictionary,
            Dictionary<KeyType, (OVRInput.RawTouch RawTouch, Subject<bool> Subject)> keepTouchDictionary,
            Dictionary<KeyType, (OVRInput.RawAxis1D RawAxis1D, Subject<float> Subject)> axis1DDictionary,
            Dictionary<KeyType, (OVRInput.RawAxis2D RawAxis2D, Subject<Vector2> Subject)> axis2DDictionary
        )
        {
            m_KeepPressSubjectDictionary = keepPressDictionary;
            m_KeepTouchSubjectDictionary = keepTouchDictionary;
            m_Axis1DSubjectDictionary = axis1DDictionary;
            m_Axis2DSubjectDictionary = axis2DDictionary;

            var buttonDowns = m_ButtonAsObservableDictionary = m_KeepPressSubjectDictionary.ToDictionary(dic => dic.Key, dic => dic.Value.Subject.DistinctUntilChanged().Publish().RefCount());
            var touchDowns = m_TouchAsObservableDictionary = m_KeepTouchSubjectDictionary.ToDictionary(dic => dic.Key, dic => dic.Value.Subject.DistinctUntilChanged().Publish().RefCount());

            m_HandInputAsObservableDictionary = new[]{
                HandInput.Grab,
                HandInput.Release
            }.ToDictionary(input => input, input =>
            {
                switch (input)
                {
                    case HandInput.Grab:
                        return buttonDowns[KeyType.HandTrigger].Where(isPressed => isPressed).AsUnitObservable().Publish().RefCount();
                    case HandInput.Release:
                        return buttonDowns[KeyType.HandTrigger].Where(isPressed => !isPressed).AsUnitObservable().Publish().RefCount();
                    default:
                        return default;
                }
            });
            var clicks = m_ClickDictionary = new[]{
                KeyType.MainButton,
                KeyType.SubButton,
                KeyType.Stick,
                KeyType.IndexTrigger
            }.ToDictionary(key => key, key => CreationClickAsObservable(buttonDowns[key].Where(isDown => isDown).AsUnitObservable()));

            m_PickAsObservable = clicks[KeyType.IndexTrigger]
                            .WithLatestFrom(buttonDowns[KeyType.HandTrigger], (isDouble, isPressed) => (isDouble, isPressed))
                            .Where(tuple => tuple.isPressed)
                            .Select(tuple => tuple.isDouble).Publish().RefCount();
        }
        public static PlayerInput CreationFromKeys((KeyType Key, OVRInput.RawButton Button)[] buttons, (KeyType Key, OVRInput.RawTouch RawTouch)[] touches, (KeyType Key, OVRInput.RawAxis1D Axis1D)[] axis1Ds, (KeyType Key, OVRInput.RawAxis2D Axis2D)[] axis2Ds)
        => new PlayerInput(
            buttons.ToDictionary(tuple => tuple.Key, tuple => (tuple.Button, new Subject<bool>())),
            touches.ToDictionary(tuple => tuple.Key, tuple => (tuple.RawTouch, new Subject<bool>())),
            axis1Ds.ToDictionary(tuple => tuple.Key, tuple => (tuple.Axis1D, new Subject<float>())),
            axis2Ds.ToDictionary(tuple => tuple.Key, tuple => (tuple.Axis2D, new Subject<Vector2>()))
        );
        public IObservable<bool> ButtonAsObservable(KeyType key) => m_ButtonAsObservableDictionary[key];
        public IObservable<bool> KeepPressAsObservable(KeyType key) => m_KeepPressSubjectDictionary[key].Subject;
        public IObservable<bool> ClickAsObservable(KeyType key) => m_ClickDictionary[key];
        public IObservable<bool> TouchAsObservable(KeyType key) => m_TouchAsObservableDictionary[key];
        public IObservable<bool> KeepTouchAsObservable(KeyType key) => m_KeepTouchSubjectDictionary[key].Subject;
        public IObservable<float> Axis1DAsObservable(KeyType key) => m_Axis1DSubjectDictionary[key].Subject;
        public IObservable<Vector2> Axis2DAsObservable(KeyType key) => m_Axis2DSubjectDictionary[key].Subject;
        public IObservable<Unit> HandInputAsObservable(HandInput handInput) => m_HandInputAsObservableDictionary[handInput];
        public IObservable<bool> PickAsObservable => m_PickAsObservable;
        public void Send()
        {
            foreach (var tuple in m_KeepPressSubjectDictionary.Values)
                tuple.Subject.OnNext(OVRInput.Get(tuple.RawButton));
            foreach (var tuple in m_KeepTouchSubjectDictionary.Values)
                tuple.Subject.OnNext(OVRInput.Get(tuple.RawTouch));
            foreach (var tuple in m_Axis1DSubjectDictionary.Values)
                tuple.Subject.OnNext(OVRInput.Get(tuple.RawAxis1D));
            foreach (var tuple in m_Axis2DSubjectDictionary.Values)
                tuple.Subject.OnNext(OVRInput.Get(tuple.RawAxis2D));
        }
        static IObservable<bool> CreationClickAsObservable(IObservable<Unit> source)
        => source.Buffer(source.Throttle(TimeSpan.FromMilliseconds(DOUBLECLICK_INTERVAL))).Select(buf => buf.Count >= 2).Publish().RefCount();
    }
}

public enum KeyType
{
    MainButton,
    SubButton,
    Stick,
    IndexTrigger,
    HandTrigger,
}
public enum InputType
{
    Button,
    Touch,
}
public enum HandInput
{
    Grab,
    // Pick,
    Release,
}
