using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using UniRx;
using Cysharp.Threading.Tasks;

public class HandGrabber : BaseGrabber, ISingleGrabber, IInteractor
{
    HandFollower m_HandFollower;
    BoolReactiveProperty m_PlayerHandRP;
    public bool PlayerHandEnabled { set { m_PlayerHandRP.Value = value; } get { return m_PlayerHandRP.Value; } }
    override public bool UseHandOffset => PlayerHandEnabled;
    HandOffset m_PlayerHandOffset;

    NetworkVariable<NetworkObject> m_NetworkTargetNV;
    public NetworkObject NetworkTarget { set { m_NetworkTargetNV.Value = value; } get { return m_NetworkTargetNV.Value; } }

    ReactiveProperty<IGrabbable> m_TargetRP = new ReactiveProperty<IGrabbable>();
    public IObservable<IGrabbable> TargetAsObservable => m_TargetRP;
    public IGrabbable Target { private set { m_TargetRP.Value = value; } get { return m_TargetRP.Value; } }
    override public void SetTarget(IGrabbable grabbable) => Target = grabbable;
    override public void RemoveTarget(IGrabbable grabbable)
    {
        if (Target == grabbable)
            Target = null;
    }
    override public bool HasTarget(IGrabbable grabbable) => Target == grabbable;

    override public void Init()
    {
        base.Init();
        m_HandFollower = GetComponentInParent<HandFollower>();
        m_NetworkTargetNV = new NetworkVariable<NetworkObject>();
        m_NetworkTargetNV.OnValueChanged += (pre, cur) => Target = cur?.GetComponent<IGrabbable>();

    }
    virtual protected void Start()
    {
        var finderAnchor = m_TargetFinder.transform;
        m_PlayerHandOffset = new HandOffset(finderAnchor.localPosition, finderAnchor.localEulerAngles);
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        m_PlayerHandRP = new BoolReactiveProperty(true);

        var finderAnchor = m_TargetFinder.transform;
        m_Subscriptions.Add(
            m_PlayerHandRP.Subscribe(enable =>
            {
                if (enable)
                {
                    finderAnchor.localPosition = m_PlayerHandOffset.Position;
                    finderAnchor.localEulerAngles = m_PlayerHandOffset.EulerAngles;
                }
                else
                {
                    finderAnchor.localPosition = default;
                    finderAnchor.localEulerAngles = default;
                }
            })
        );
    }
    override public void OnPool()
    {
        m_PlayerHandRP.Dispose();
        base.OnPool();
    }
    override public void Release() => Target?.Release(this);
    override public void AddForce(Vector3 force) => m_HandFollower.AddForce(force);
}
