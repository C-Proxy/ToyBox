using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrabObservable : BaseObservable
{
    UnityEvent<IGrabber> m_GrabEvent = new UnityEvent<IGrabber>();
    public void SetGrabEvent(UnityAction<IGrabber> action) => m_GrabEvent.AddListener(action);
    public void Grab(IGrabber grabber) => m_GrabEvent.Invoke(grabber);

    override public void OnPool()
    {
        m_GrabEvent.RemoveAllListeners();
    }
}