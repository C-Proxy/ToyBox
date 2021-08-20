using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaycastEventHandler : BaseEventReceiver, IRaycastEventHandler
{
    UnityEvent<FocusEvent> m_FocusEvent = new UnityEvent<FocusEvent>();
    UnityEvent<InteractEvent> m_InteractEvent = new UnityEvent<InteractEvent>();
    UnityEvent<DamageEvent> m_DamageEvent = new UnityEvent<DamageEvent>();
    public void SetEvent(UnityAction<FocusEvent> action) => m_FocusEvent.AddListener(action);
    public void SetEvent(UnityAction<InteractEvent> action) => m_InteractEvent.AddListener(action);
    public void SetEvent(UnityAction<DamageEvent> action) => m_DamageEvent.AddListener(action);
    public void RemoveEvent()
    {
        m_FocusEvent.RemoveAllListeners();
        m_InteractEvent.RemoveAllListeners();
        m_DamageEvent.RemoveAllListeners();
    }
    public void SendEvent(FocusEvent info) => m_FocusEvent.Invoke(info);
    public void SendEvent(InteractEvent info) => m_InteractEvent.Invoke(info);
    public void SendEvent(DamageEvent info) => m_DamageEvent.Invoke(info);
    override public void OnPool()
    {
        RemoveEvent();
        base.OnPool();
    }
}
