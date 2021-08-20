using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GunSpace;

public class ReloadEventHandler : BaseEventHandler<ReloadEvent>
{
    [SerializeField] BulletType m_BulletType = default;
    public bool IsReloadable(BulletType type) => IsActive && m_BulletType == type;
    public void SendReloadEvent(IEventSource source, BulletType type, int count) => SendEvent(new ReloadEvent(source, type, count));
}