using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Prefab;
using CoinSpace;

public class MyDebugger : MonoBehaviour
{
    [SerializeField] BaseGrabber Grabber = default;
    [SerializeField] BaseItem TargetItem = default;
    [ContextMenu("ForceGrab")]
    public void ForceGrab()
    {
        if (Grabber?.NetworkBehaviour)
            TargetItem?.TryGrabServerRpc(NetworkInfo.CreateFrom(Grabber.NetworkBehaviour));
    }
}
