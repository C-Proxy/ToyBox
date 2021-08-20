using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class RaycastableColliderHandler : BaseEventReceiver, IRaycastReceivable
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

    public void SendEvent(FocusEvent info) => m_FocusEventHandler?.SendEvent(info);
    public void SendEvent(InteractEvent info) => m_InteractEventHandler?.SendEvent(info);
    public void SendEvent(DamageEvent info) => m_DamageEventHandler?.SendEvent(info);

}