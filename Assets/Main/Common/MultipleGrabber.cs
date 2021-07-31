using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

public class MultipleGrabber : BaseGrabber, IMultipleGrabber
{
    override public bool UseHandOffset => false;
    ReactiveCollection<IGrabbable> m_TargetRC;
    public IGrabbable[] Targets => m_TargetRC.ToArray();
    override public void SetTarget(IGrabbable grabbable) => m_TargetRC.Add(grabbable);
    virtual public IGrabbable GetTarget(int index) => m_TargetRC[index];
    override public void RemoveTarget(IGrabbable grabbable) => m_TargetRC.Remove(grabbable);
    override public bool HasTarget(IGrabbable grabbable) => m_TargetRC.Contains(grabbable);
    override public void Release() => m_TargetRC.Clear();

    override public void OnSpawn()
    {
        m_TargetRC = new ReactiveCollection<IGrabbable>();
    }
    override public void OnPool()
    {
        m_TargetRC.Dispose();
    }
}
