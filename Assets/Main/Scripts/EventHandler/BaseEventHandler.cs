using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;

abstract public class BaseEventHandler<T> : BaseEventReceiver<T>, IEventHandler<T>
{
    UnityEvent<T> m_Event = new UnityEvent<T>();
    public void SetEvent(UnityAction<T> action) => m_Event.AddListener(action);
    public void RemoveEvent() => m_Event.RemoveAllListeners();
    override public void SendEvent(T info) => m_Event.Invoke(info);
    override public void OnPool()
    {
        RemoveEvent();
        base.OnPool();
    }
}
abstract public class BaseEventHandler<TReceive, TEvent> : BaseEventReceiver<TReceive>, IEventHandler<TEvent>
{
    UnityEvent<TEvent> m_Event = new UnityEvent<TEvent>();
    public void SetEvent(UnityAction<TEvent> action) => m_Event.AddListener(action);
    public void RemoveEvent() => m_Event.RemoveAllListeners();
    abstract public void SendEvent(TEvent info);
    override public void OnPool()
    {
        RemoveEvent();
        base.OnPool();
    }
}
abstract public class BaseEventHandler<TReceive, TEvent1, TEvent2> : BaseEventReceiver<TReceive>, IEventHandler<TReceive, TEvent1, TEvent2>
{
    protected UnityEvent<TEvent1> m_Event1 = new UnityEvent<TEvent1>();
    protected UnityEvent<TEvent2> m_Event2 = new UnityEvent<TEvent2>();
    public void SetEvent(UnityAction<TEvent1> action) => m_Event1.AddListener(action);
    public void SetEvent(UnityAction<TEvent2> action) => m_Event2.AddListener(action);

    public void RemoveEvent()
    {
        m_Event1.RemoveAllListeners();
        m_Event2.RemoveAllListeners();
    }
    override public void OnPool()
    {
        RemoveEvent();
        base.OnPool();
    }
}
abstract public class BaseEventHandler<TReceive, TEvent1, TEvent2, TEvent3> : BaseEventReceiver<TReceive>, IEventHandler<TReceive, TEvent1, TEvent2, TEvent3>
{
    protected UnityEvent<TEvent1> m_Event1 = new UnityEvent<TEvent1>();
    protected UnityEvent<TEvent2> m_Event2 = new UnityEvent<TEvent2>();
    protected UnityEvent<TEvent3> m_Event3 = new UnityEvent<TEvent3>();

    public void SetEvent(UnityAction<TEvent1> action) => m_Event1.AddListener(action);
    public void SetEvent(UnityAction<TEvent2> action) => m_Event2.AddListener(action);
    public void SetEvent(UnityAction<TEvent3> action) => m_Event3.AddListener(action);


    public void RemoveEvent()
    {
        m_Event1.RemoveAllListeners();
        m_Event2.RemoveAllListeners();
        m_Event3.RemoveAllListeners();
    }
    override public void OnPool()
    {
        RemoveEvent();
        base.OnPool();
    }
}
[RequireComponent(typeof(Collider))]
abstract public class BaseEventColliderHandler<T> : BaseEventReceiver<T>
{
    IEventHandler<T> m_EventHandler;
    private void Awake()
    {
        m_EventHandler = GetComponentInParent<IEventHandler<T>>();
    }
    override public void SendEvent(T info) => m_EventHandler.SendEvent(info);
}
abstract public class BaseEventReceiver<T> : NetworkPoolableChild, IEventReceivable<T>
{
    [SerializeField] bool m_IsActive;
    public bool IsActive { set { m_IsActive = value; } get { return m_IsActive; } }
    abstract public void SendEvent(T info);
}
