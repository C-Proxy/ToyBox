using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public interface IGrabbable
{
    bool IsGrabbed { get; }
    IGrabber Parent { get; }
    Transform transform { get; }
    HandShapeHandler HandShapeHandler { get; }
    bool RequestChangeParent(IGrabber parent);
    void Release(IGrabber parent);
    void ForceRelease();
    void OnGrab(IGrabber grabber);
    void OnRelease(IGrabber grabber);
}
