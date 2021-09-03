using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonEventHandler : BaseButtonEventHandler<UnitEvent>
{
    [SerializeField] SpriteManager.LaserIcon m_FocusIcon = default;
    [SerializeField] string m_FocusText = default;
    override protected SpriteManager.LaserIcon GetFocusIcon() => m_FocusIcon;
    override protected string GetFocusText() => m_FocusText;
    override protected UnitEvent CreateSendInfo(ActionEvent info) => new UnitEvent(null);
    override public void SendEvent(InteractEvent info)
    {
        base.SendEvent(info);
    }
}
abstract public class BaseButtonEventHandler<T> : BaseEventHandler<T>, IRaycastReceivable
{
    abstract protected SpriteManager.LaserIcon GetFocusIcon();
    abstract protected string GetFocusText();
    abstract protected T CreateSendInfo(ActionEvent info);
    public void SendEvent(FocusEvent info)
    {
        info.Laser.SetSpriteAndText(GetFocusIcon(), GetFocusText());
    }
    virtual public void SendEvent(InteractEvent info)
    {
        SendEvent(CreateSendInfo(info));
    }
    virtual public void SendEvent(DamageEvent info) { }
}