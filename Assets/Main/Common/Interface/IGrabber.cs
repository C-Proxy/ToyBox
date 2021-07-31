using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Serialization;
using MLAPI.Spawning;

public interface IGrabber
{
    bool UseHandOffset { get; }
    HandDominant HandDominant { get; }
    NetworkBehaviour NetworkBehaviour { get; }
    Transform GrabAnchor { get; }
    void Set(NetworkBehaviour networkParent);
    void Set(NetworkBehaviour networkParent, HandDominant handDominant);
    void OnGrab(IGrabbable grabbable);
    void SetTarget(IGrabbable grabbable);
    void RemoveTarget(IGrabbable grabbable);
    bool HasTarget(IGrabbable grabbable);
    void SendGrabAction();
}
public interface ISingleGrabber : IGrabber
{
    IGrabbable Target { get; }
}
public interface IMultipleGrabber : IGrabber
{
    IGrabbable GetTarget(int index);
    IGrabbable[] Targets { get; }
}
