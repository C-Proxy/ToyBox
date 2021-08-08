using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prefab;

[RequireComponent(typeof(Rigidbody))]
public class MockObject : LocalPoolableParent
{
    override public LocalPrefabName PrefabName => LocalPrefabName.Mock_WingmanAmmo;
    Rigidbody m_Rigidbody;
    public Rigidbody Rigidbody => m_Rigidbody;

    protected void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
}
