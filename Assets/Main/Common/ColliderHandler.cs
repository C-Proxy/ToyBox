using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public struct ColliderHandler
{
    [SerializeField] Collider[] m_Colliders;
    public void SetActiveCollision(bool active)
    {
        foreach (var collider in m_Colliders)
            collider.isTrigger = !active;
    }
}
