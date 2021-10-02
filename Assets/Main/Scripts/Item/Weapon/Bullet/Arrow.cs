using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using MLAPI.NetworkVariable;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;

public class Arrow : BaseItem, IDamageSource
{
    const float DAMAGE_VALUE = 0f;
    const float STICK_DEPTH = 0f;
    const float STICK_ANGLE = 0.8f;
    [SerializeField] Collider m_TailCollider = default;
    Transform m_TailAnchor;
    public Transform TailAnchor => m_TailAnchor;
    ulong m_OwnerId;
    public ulong OwnerId => m_OwnerId;
    CancellationTokenSource m_ProjectCTS;
    override protected void Awake()
    {
        base.Awake();
        m_TailAnchor = m_TailCollider.transform;
        m_TailCollider.OnTriggerEnterAsObservable().Subscribe(other =>
        {
            if (IsOwner && other.TryGetComponent<ArrowHandle>(out var handle))
            {
                handle.SendArrowEvent(this);
            }
        }).AddTo(this);
    }
    override public void OnGrab(IGrabber parent)
    {
        base.OnGrab(parent);
        m_TailCollider.enabled = true;
        m_ProjectCTS?.Cancel();
    }
    override public void OnRelease(IGrabber parent)
    {
        base.OnRelease(parent);
        m_TailCollider.enabled = false;
    }
    async public UniTaskVoid ProjectAsync(ulong owner, float firstVelocity)
    {
        if (m_ProjectCTS != null) return;
        m_ProjectCTS = new CancellationTokenSource();
        m_Rigidbody.velocity = transform.forward * firstVelocity;
        m_Rigidbody.angularVelocity = default;
        m_OwnerId = owner;
        RaycastHit hitInfo;
        try
        {
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate())
            {
                transform.rotation = Quaternion.LookRotation(m_Rigidbody.velocity, Vector3.up);
                m_AliveCTS.Token.ThrowIfCancellationRequested();
                m_ProjectCTS.Token.ThrowIfCancellationRequested();
                if (Physics.Raycast(transform.position, m_Rigidbody.velocity, out hitInfo, m_Rigidbody.velocity.magnitude, (int)LayerName.RaycastTarget))
                {
                    if (hitInfo.collider.TryGetComponent<IEventReceivable<DamageEvent>>(out var eventReceivable))
                    {
                        eventReceivable.SendEvent(new DamageEvent(this, DAMAGE_VALUE));
                    }
                    else
                    {
                        if (Vector3.Dot(m_Rigidbody.velocity.normalized, -hitInfo.normal) > STICK_ANGLE)
                        {

                        }
                        break;
                    }
                }
            }
        }
        catch (OperationCanceledException) { throw; }
        finally
        {
            m_ProjectCTS?.Cancel();
            m_ProjectCTS = null;
        }
    }
    public void ResetMeshAnchor() => m_TailAnchor.localRotation = Quaternion.identity;
    public void OnDealDamage() => m_ProjectCTS?.Cancel();
}
