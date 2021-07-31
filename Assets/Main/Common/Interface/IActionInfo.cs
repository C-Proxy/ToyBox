using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInteractor
{

}
public interface ILaserReceivable
{
    bool IsTargettable { set; get; }
    void Interact(IInteractor interactor, IActionInfo info);
    void SendFocusInfo(LaserTargetFinder laser, IGrabbable grabItem);
    // void SetFocusEvent(UnityAction<LaserFocusInfo> action);
}
public interface IActionInfo
{

}
public readonly struct LaserAction : IActionInfo
{
    public readonly bool IsDouble;
    public LaserAction(bool isDouble)
    {
        IsDouble = isDouble;
    }
}