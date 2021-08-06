using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReloadEventHandler : ActionEventHandler
{
    [SerializeField] BulletType m_BulletType = default;
    public bool IsReloadable(BulletType type) => IsTargettable && m_BulletType == type;
}