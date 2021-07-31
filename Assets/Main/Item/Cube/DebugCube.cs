using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prefab;

public class DebugCube : BaseItem
{
    override public void OnGrab(IGrabber parent)
    {
        base.OnGrab(parent);
        Debug.Log("OnGrab");
    }
    override public void OnRelease(IGrabber grabber)
    {
        base.OnRelease(grabber);
        Debug.Log("OnRelease");
    }
}
