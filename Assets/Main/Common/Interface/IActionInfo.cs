using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInteractor
{

}
public interface IDamageDealer : IInteractor
{
    void OnDealDamage();
}
public interface ILaserReceivable
{
    bool IsTargettable { set; get; }
    void Interact(IInteractor interactor, IActionInfo info);
    void SendFocusInfo(LaserTargetFinder laser, IGrabbable grabItem);
}
public interface IActionInfo { }
public readonly struct LaserAction : IActionInfo
{
    public readonly bool IsDouble;
    public LaserAction(bool isDouble)
    {
        IsDouble = isDouble;
    }
}
public readonly struct ReloadAction : IActionInfo
{
    public readonly BulletType BulletType;
    public readonly int Count;
    public ReloadAction(BulletType type, int count)
    {
        BulletType = type;
        Count = count;
    }
}
public readonly struct DamageAction : IActionInfo
{
    public readonly float Value;
    public DamageAction(float value)
    {
        Value = value;
    }
}