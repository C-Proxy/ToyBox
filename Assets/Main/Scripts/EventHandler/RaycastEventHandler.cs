using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaycastEventHandler : BaseEventReceiver<RaycastEvent>, IRaycastEventHandler
{
    UnityEvent<FocusEvent> m_FocusEvent = new UnityEvent<FocusEvent>();
    UnityEvent<InteractEvent> m_InteractEvent = new UnityEvent<InteractEvent>();
    UnityEvent<DamageEvent> m_DamageEvent = new UnityEvent<DamageEvent>();
    public void SetEvent(UnityAction<FocusEvent> focusAction) => m_FocusEvent.AddListener(focusAction);
    public void SetEvent(UnityAction<InteractEvent> interactAction) => m_InteractEvent.AddListener(interactAction);
    public void SetEvent(UnityAction<DamageEvent> damageAction) => m_DamageEvent.AddListener(damageAction);
    public void RemoveEvent()
    {
        m_FocusEvent.RemoveAllListeners();
        m_InteractEvent.RemoveAllListeners();
        m_DamageEvent.RemoveAllListeners();
    }
    override public void SendEvent(RaycastEvent info)
    {
        switch (info)
        {
            case FocusEvent focusInfo:
                m_FocusEvent?.Invoke(focusInfo);
                break;
            case InteractEvent interactInfo:
                m_InteractEvent?.Invoke(interactInfo);
                break;
            case DamageEvent damageInfo:
                m_DamageEvent?.Invoke(damageInfo);
                break;
        }
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
