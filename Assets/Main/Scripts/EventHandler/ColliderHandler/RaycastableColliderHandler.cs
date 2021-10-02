using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class RaycastableColliderHandler : BaseEventHandler<RaycastEvent, FocusEvent, InteractEvent, DamageEvent>
{
    FocusEventHandler m_FocusEventHandler;
    InteractEventHandler m_InteractEventHandler;
    DamageEventHandler m_DamageEventHandler;
    private void Awake()
    {
        m_FocusEventHandler = GetComponentInParent<FocusEventHandler>();
        m_InteractEventHandler = GetComponentInParent<InteractEventHandler>();
        m_DamageEventHandler = GetComponentInParent<DamageEventHandler>();
    }
    override public void SendEvent(RaycastEvent info)
    {
        switch (info)
        {
            case FocusEvent focusInfo:
                m_FocusEventHandler?.SendEvent(focusInfo);
                break;
            case InteractEvent interactInfo:
                m_InteractEventHandler?.SendEvent(interactInfo);
                break;
            case DamageEvent damageInfo:
                m_DamageEventHandler?.SendEvent(damageInfo);
                break;
        }
    }

    // public void SendEvent(FocusEvent info) => m_FocusEventHandler?.SendEvent(info);
    // public void SendEvent(InteractEvent info) => m_InteractEventHandler?.SendEvent(info);
    // public void SendEvent(DamageEvent info) => m_DamageEventHandler?.SendEvent(info);

}