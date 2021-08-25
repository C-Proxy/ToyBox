using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using UniRx;
using Cysharp.Threading.Tasks;

abstract public class BaseGrabber : NetworkPoolableChild, IGrabber, IEventSource
{
    abstract public bool UseHandOffset { get; }
    HandDominant m_HandDominant;
    public HandDominant HandDominant => m_HandDominant;
    public NetworkBehaviour NetworkBehaviour => this;
    protected ITargetFinder m_TargetFinder;
    public ITargetFinder TargetFinder => m_TargetFinder;
    virtual public Transform GrabAnchor => transform;
    override public void Init()
    {
        base.Init();
        m_TargetFinder = GetComponentInChildren<ITargetFinder>();
    }
    public void Set(NetworkBehaviour networkParent) { }// => m_NetworkParent = networkParent;
    public void Set(NetworkBehaviour networkBehaviour, HandDominant handDominant)
    {
        // m_NetworkParent = networkBehaviour;
        // m_HandDominant = handDominant;
    }
    abstract public void SetTarget(IGrabbable grabbable);
    abstract public void RemoveTarget(IGrabbable grabbable);
    abstract public bool HasTarget(IGrabbable grabbable);
    public void SendGrabEvent() => m_TargetFinder.FindTarget<GrabEvent>((int)LayerName.GrabTarget)?.SendEvent(new GrabEvent(this));
    abstract public void ForceRecoil(Recoil recoil);

    abstract public void Release();
    virtual public void OnGrab(IGrabbable grabbable) { }

}
public enum HandDominant
{
    Left,
    Right,
    Either,
}