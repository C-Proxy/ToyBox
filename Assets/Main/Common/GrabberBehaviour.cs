using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prefab;

abstract public class GrabberBehaviour : NetworkPoolableBehaviour
{
    protected List<IGrabber> m_Grabbers = new List<IGrabber>();

    public void AppendGrabber(IGrabber grabber, HandDominant handDominant)
    {
        m_Grabbers.Add(grabber);
        grabber.Set(this, handDominant);
    }
    public void RemoveGrabber(IGrabber grabber)
    {
        m_Grabbers.Remove(grabber);
        grabber.Set(null);
    }
    public void ClearGrabber()
    {
        foreach (var grabber in m_Grabbers)
            grabber.Set(null);
        m_Grabbers.Clear();

    }
}
