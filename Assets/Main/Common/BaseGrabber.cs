using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using UniRx;
using Cysharp.Threading.Tasks;

abstract public class BaseGrabber : NetworkPoolableChildBehaviour, IGrabber, IInteractor
{
    abstract public bool UseHandOffset { get; }
    HandDominant m_HandDominant;
    public HandDominant HandDominant => m_HandDominant;
    public NetworkBehaviour NetworkBehaviour => this;
    protected ITargetFinder m_TargetFinder;
    public ITargetFinder TargetFinder => m_TargetFinder;
    virtual public Transform GrabAnchor => transform;
    virtual protected void Awake()
    {
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
    public void SendGrabAction() => m_TargetFinder.FindTarget<GrabEventHandler>((int)LayerName.GrabTarget)?.Grab(this);

    abstract public void Release();
    virtual public void OnGrab(IGrabbable grabbable) { }

}
public enum HandDominant
{
    Left,
    Right,
    Either,
}