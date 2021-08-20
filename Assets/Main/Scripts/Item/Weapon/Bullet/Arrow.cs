using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

public class Arrow : BaseItem
{
    [SerializeField] Collider m_TailCollider = default;
    Transform m_TailAnchor;
    public Transform TailAnchor => m_TailAnchor;

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
    async public UniTaskVoid ProjectAsync(float firstVelocity)
    {
        if (m_ProjectCTS != null) return;
        m_ProjectCTS = new CancellationTokenSource();
        m_Rigidbody.velocity = transform.forward * firstVelocity;
        m_Rigidbody.angularVelocity = Vector3.zero;
        try
        {
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate())
            {
                transform.rotation = Quaternion.LookRotation(m_Rigidbody.velocity, Vector3.up);
                m_AliveCTS.Token.ThrowIfCancellationRequested();
                m_ProjectCTS.Token.ThrowIfCancellationRequested();
                await UniTask.Yield();
            }
        }
        catch (OperationCanceledException)
        {

        }
        finally
        {
            m_ProjectCTS = null;
        }
    }
    public void ResetMeshAnchor() => m_TailAnchor.localRotation = Quaternion.identity;
    private void OnCollisionEnter(Collision other)
    {
        m_ProjectCTS?.Cancel();
    }
}
