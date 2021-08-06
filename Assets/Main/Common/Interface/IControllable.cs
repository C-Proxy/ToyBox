using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllable
{
    void Connect(InputManager.HandInput input);
}
/*
public interface IControllable : IStickControllable, IMainButtonControllable, ISubButtonControllable, IHandTriggerControllable, IIndexTriggerControllable, IOtherButtonControllable { }
public interface IMainButtonControllable
{
    void OnMainUp();
    void OnClickMain(bool isDouble);
    void OnTouchDownMain();
    void OnTouchUpMain();
}
public interface ISubButtonControllable
{
    void OnSubUp();
    void OnClickSub(bool isDouble);
    void OnTouchSub(bool isPressed);
}
public interface IHandTriggerControllable
{
    void OnInputHand(float value);
    void OnPressHand(bool isPressed);

}
public interface IIndexTriggerControllable
{
    void OnInputIndex(float value);
    void OnPressIndex(bool isPressed);
    void OnTouchIndex(bool isPressed);
}
public interface IOtherButtonControllable
{
    void OnPressMenuDown();
    //void OnHomeButtonDown();
}
public interface IPositionControllable
{
    void OnInputPosition(Vector3 position);
    void OnInputRotation(Quaternion rotation);
}
public interface IKeyControllable : IMainButtonControllable, ISubButtonControllable, IHandTriggerControllable, IIndexTriggerControllable, IOtherButtonControllable { }
public interface IStickControllable
{
    void OnInputStick(Vector2 axis);
    void OnPressStick(bool isPressed);
    void OnTouchStick(bool isPressed);
}
*/