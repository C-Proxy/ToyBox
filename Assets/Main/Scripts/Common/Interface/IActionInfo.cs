using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GunSpace;

abstract public class ActionEvent
{
    public readonly IEventSource EventSource;
    public ActionEvent(IEventSource eventSource)
    {
        EventSource = eventSource;
    }
}
public interface IEventSource { }
public interface IDamageSource : IEventSource
{
    ulong OwnerId { get; }
    void OnDealDamage();
}
public class RaycastEvent : ActionEvent
{
    public RaycastEvent(IEventSource source) : base(source) { }
}
public class UnitEvent : ActionEvent
{
    public UnitEvent(IEventSource source) : base(source) { }
}
public class ValueEvent<T> : ActionEvent
{
    public readonly T Value;
    public ValueEvent(IEventSource source, T value) : base(source)
    {
        Value = value;
    }
}
public class FocusEvent : RaycastEvent
{
    public LaserTargetFinder Laser => EventSource as LaserTargetFinder;
    public readonly IGrabbable GrabItem;
    public FocusEvent(LaserTargetFinder source, IGrabbable item) : base(source)
    {
        GrabItem = item;
    }
}
public class InteractEvent : RaycastEvent
{
    public InteractEvent(IEventSource source) : base(source) { }
}
public class GrabEvent : ActionEvent
{
    public IGrabber Grabber => EventSource as IGrabber;
    public GrabEvent(IGrabber grabber) : base(grabber) { }
}
public class ReloadEvent : ActionEvent
{
    public readonly BulletType BulletType;
    public readonly int Count;
    public ReloadEvent(IEventSource source, BulletType type, int count) : base(source)
    {
        BulletType = type;
        Count = count;
    }
}
public class DamageEvent : RaycastEvent
{
    public readonly IDamageSource DamageSource;
    public readonly float Value;
    public DamageEvent(IDamageSource source, float value) : base(source)
    {
        DamageSource = source;
        Value = value;
    }
}

public interface IEventReceivable<T>
{
    bool IsActive { set; get; }
    void SendEvent(T info);
}
public interface IEventHandler<T> : IEventReceivable<T>
{
    void SetEvent(UnityAction<T> action);
    void RemoveEvent();
}
public interface IRaycastReceivable : IEventReceivable<InteractEvent>, IEventReceivable<FocusEvent>, IEventReceivable<DamageEvent> { }
public interface IRaycastEventHandler : IEventHandler<InteractEvent>, IEventHandler<FocusEvent>, IEventReceivable<DamageEvent>, IRaycastReceivable { }