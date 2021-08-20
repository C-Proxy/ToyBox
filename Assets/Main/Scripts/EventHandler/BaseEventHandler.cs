using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;

abstract public class BaseEventHandler<T> : BaseEventReceiver, IEventHandler<T>
where T : IActionEvent
{
    UnityEvent<T> m_Event = new UnityEvent<T>();
    public void SetEvent(UnityAction<T> action) => m_Event.AddListener(action);
    public void RemoveEvent() => m_Event.RemoveAllListeners();
    public void SendEvent(T info) => m_Event.Invoke(info);
    override public void OnPool()
    {
        RemoveEvent();
        base.OnPool();
    }
}
[RequireComponent(typeof(Collider))]
abstract public class BaseEventColliderHandler<T> : BaseEventReceiver, IEventReceivable<T>
where T : IActionEvent
{
    IEventHandler<T> m_EventHandler;
    private void Awake()
    {
        m_EventHandler = GetComponentInParent<IEventHandler<T>>();
    }
    public void SendEvent(T info) => m_EventHandler.SendEvent(info);
}
abstract public class BaseEventReceiver : LocalPoolableChild
{
    [SerializeField] bool m_IsActive;
    public bool IsActive { set { m_IsActive = value; } get { return m_IsActive; } }
}
