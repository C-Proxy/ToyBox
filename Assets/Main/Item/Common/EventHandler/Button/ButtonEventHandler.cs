using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonEventHandler : BaseEventHandler, ILaserReceivable
{
    [SerializeField] SpriteManager.LaserIcon m_FocusIcon = default;
    [SerializeField] string m_FocusText = default;

    override public void OnPool()
    {
        m_ButtonEvent.RemoveAllListeners();
        base.OnPool();
    }

    UnityEvent m_ButtonEvent = new UnityEvent();
    public void SetPressEvent(UnityAction action) => m_ButtonEvent.AddListener(action);
    public void Interact(IInteractor interactor, IActionInfo info) => m_ButtonEvent.Invoke();
    public void SendFocusInfo(LaserTargetFinder laser, IGrabbable grabbable)
    => laser.SetSpriteAndText(m_FocusIcon, m_FocusText);
}