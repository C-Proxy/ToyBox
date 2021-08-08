using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(Collider))]
abstract public class BaseEventHandler : LocalPoolableChild
{
    public bool IsActive = true;
    public bool IsTargettable { set { IsActive = value; } get { return IsActive; } }
}
