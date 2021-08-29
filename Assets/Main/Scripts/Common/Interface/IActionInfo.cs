using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GunSpace;

public interface IActionEvent { }
public interface IEventSource { }
public interface IDamageSource
{
    ulong OwnerId { get; }
    void OnDealDamage();
}
public interface IRaycastEvent : IActionEvent { }
public readonly struct UnitEvent : IActionEvent { }
public readonly struct BooleanEvent : IActionEvent
{
    public readonly bool Value;
    public BooleanEvent(bool value)
    {
        Value = value;
    }
}
public readonly struct FocusEvent : IRaycastEvent
{
    public readonly LaserTargetFinder Laser;
    public readonly IGrabbable GrabItem;
    public FocusEvent(LaserTargetFinder laser, IGrabbable item)
    {
        Laser = laser;
        GrabItem = item;
    }
}
public readonly struct InteractEvent : IRaycastEvent
{
    public readonly IEventSource Interactor;
    public InteractEvent(IEventSource source)
    {
        Interactor = source;
    }
}
public readonly struct GrabEvent : IActionEvent
{
    public readonly IGrabber Grabber;
    public GrabEvent(IGrabber grabber)
    {
        Grabber = grabber;
    }
}
public readonly struct ReloadEvent : IActionEvent
{
    public readonly IEventSource EventSource;
    public readonly BulletType BulletType;
    public readonly int Count;
    public ReloadEvent(IEventSource source, BulletType type, int count)
    {
        EventSource = source;
        BulletType = type;
        Count = count;
    }
}
public readonly struct DamageEvent : IRaycastEvent
{
    public readonly IDamageSource DamageSource;
    public readonly float Value;
    public DamageEvent(IDamageSource source, float value)
    {
        DamageSource = source;
        Value = value;
    }
}

public interface IEventReceivable<T>
where T : IActionEvent
{
    bool IsActive { set; get; }
    void SendEvent(T info);
}
public interface IEventHandler<T> : IEventReceivable<T> where T : IActionEvent
{
    void SetEvent(UnityAction<T> action);
    void RemoveEvent();
}
public interface IRaycastReceivable : IEventReceivable<InteractEvent>, IEventReceivable<FocusEvent>, IEventReceivable<DamageEvent> { }
public interface IRaycastEventHandler : IEventHandler<InteractEvent>, IEventHandler<FocusEvent>, IEventReceivable<DamageEvent>, IRaycastReceivable { }