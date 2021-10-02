using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MLAPI.NetworkVariable;

public class ThermoEventHandler : BaseEventHandler<ThermoEvent, ValueEvent<bool>, ValueEvent<float>>
{
    NetworkVariableFloat m_TemparatureNV = new NetworkVariableFloat();
    float Temparature { set { m_TemparatureNV.Value = value; } get { return m_TemparatureNV.Value; } }
    [SerializeField] float m_SpecificHeat = 1;
    [SerializeField] float m_IgniteTemparature = default;
    override public void SendEvent(ThermoEvent info)
    {

    }


}
