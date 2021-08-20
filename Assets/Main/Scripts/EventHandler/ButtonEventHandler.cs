using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonEventHandler : BaseButtonEventHandler<UnitEvent>, IRaycastReceivable
{
    [SerializeField] SpriteManager.LaserIcon m_FocusIcon = default;
    [SerializeField] string m_FocusText = default;
    override protected SpriteManager.LaserIcon GetFocusIcon() => m_FocusIcon;
    override protected string GetFocusText() => m_FocusText;
    override protected UnitEvent SendInfo => new UnitEvent();
    override public void SendEvent(InteractEvent info)
    {
        base.SendEvent(info);
    }
}
abstract public class BaseButtonEventHandler<T> : BaseEventHandler<T>, IRaycastReceivable
where T : IActionEvent
{
    abstract protected SpriteManager.LaserIcon GetFocusIcon();
    abstract protected string GetFocusText();
    abstract protected T SendInfo { get; }
    public void SendEvent(FocusEvent info)
    {
        info.Laser.SetSpriteAndText(GetFocusIcon(), GetFocusText());
    }
    virtual public void SendEvent(InteractEvent info)
    {
        SendEvent(SendInfo);
    }
    virtual public void SendEvent(DamageEvent info) { }
}