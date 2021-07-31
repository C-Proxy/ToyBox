using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using MLAPIPlayerSpace;

public interface IGrabbable
{
    bool IsGrabbed { get; }
    IGrabber Parent { get; }
    Transform transform { get; }
    HandShapeHandler HandShapeHandler { get; }
    IObservable<HandShape> HandShapeAsObservable { get; }
    void RequestChangeParent(IGrabber parent);
    void Release(IGrabber parent);
    [ServerRpc]
    void ForceReleaseServerRpc();
    void OnGrab(IGrabber grabber);
    void OnRelease(IGrabber grabber);
}
