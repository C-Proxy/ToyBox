using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prefab;

[RequireComponent(typeof(Rigidbody))]
public class DebugAxis : LocalPoolableBehaviour
{
    Rigidbody m_Rigidbody;
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    [ContextMenu("Pool")]
    override public void OnPool()
    {
        SetVelocity(default, default);
        base.OnPool();
    }
    public void SetVelocity(Vector3 velocity, Vector3 angularVelocity)
    {
        m_Rigidbody.velocity = velocity;
        m_Rigidbody.angularVelocity = angularVelocity;
    }
    public static GameObject Generate(Vector3 position, Quaternion rotation)
    => PrefabGenerator.GenerateLocalPrefab(LocalPrefabName.Debug_Axis, position, rotation);
    public static void Generate(Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
    {
        var axis = Generate(position, rotation).GetComponent<DebugAxis>();
        axis.SetVelocity(velocity, angularVelocity);
    }
}
