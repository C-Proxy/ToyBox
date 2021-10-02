using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HeatSource : MonoBehaviour, IEventSource
{
    [SerializeField] float m_Intensity = default, m_Temparature = default;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<IEventReceivable<ThermoEvent>>(out var receiver))
        {
            receiver.SendEvent(new ThermoEvent(this, m_Intensity, m_Temparature));
        }
    }
}
