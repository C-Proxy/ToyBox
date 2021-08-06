using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prefab;

[RequireComponent(typeof(Rigidbody))]
public class MockObject : LocalPoolableBehaviour
{
    override public LocalPrefabName PrefabName => LocalPrefabName.Mock_WingmanAmmo;
    Rigidbody m_Rigidbody;
    public Rigidbody Rigidbody => m_Rigidbody;

    override protected void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        base.Awake();
    }
}
