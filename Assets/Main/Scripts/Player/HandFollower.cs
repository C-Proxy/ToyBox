using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

[RequireComponent(typeof(Rigidbody))]
public class HandFollower : LocalPoolableChild
{
    [SerializeField] float STABILITY = 10f;
    [SerializeField] float SQR_RADIUS = 0.001f;
    Rigidbody m_Rigidbody;
    public Rigidbody Rigidbody => m_Rigidbody;
    public bool IsSleep => m_Rigidbody.isKinematic;
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    public void AddForce(Recoil recoil)
    {
        if (IsSleep)
        {
            m_Rigidbody.isKinematic = false;
            FollowAsync().Forget();
        }
        m_Rigidbody.AddForce(recoil.Force, ForceMode.Impulse);
        m_Rigidbody.AddTorque(recoil.Torque, ForceMode.Impulse);
    }
    async UniTaskVoid FollowAsync()
    {
        var token = m_AliveCTS.Token;
        try
        {
            await UniTask.Yield();
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate())
            {
                var localPosition = transform.TransformVector(transform.localPosition);
                var force = -localPosition * STABILITY;
                m_Rigidbody.AddForce(force, ForceMode.Acceleration);
                m_Rigidbody.angularVelocity *= 0.8f;
                transform.localRotation = Quaternion.Lerp(transform.localRotation, default, 0.2f);
                if (Vector3.Dot(force, m_Rigidbody.velocity) > 0 && localPosition.sqrMagnitude < SQR_RADIUS)
                    break;
                token.ThrowIfCancellationRequested();
            }
        }
        catch (OperationCanceledException) { throw; }
        m_Rigidbody.isKinematic = true;
        transform.localPosition = default;
        transform.localRotation = default;
        m_Rigidbody.velocity = default;
        m_Rigidbody.angularVelocity = default;
    }
}