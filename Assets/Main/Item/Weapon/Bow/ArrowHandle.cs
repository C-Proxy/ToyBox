using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArrowHandle : MonoBehaviour
{
    UnityEvent<Arrow> m_ArrowEvent = new UnityEvent<Arrow>();
    public void SetArrowEvent(UnityAction<Arrow> action) => m_ArrowEvent.AddListener(action);
    public void SendArrowEvent(Arrow arrow) => m_ArrowEvent.Invoke(arrow);
}
