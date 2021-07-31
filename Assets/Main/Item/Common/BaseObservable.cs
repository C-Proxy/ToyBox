using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(Collider))]
abstract public class BaseObservable : PoolableChildBehaviour
{
    public bool m_IsTargettable = true;
    public bool IsTargettable { set { m_IsTargettable = value; } get { return m_IsTargettable; } }
}
