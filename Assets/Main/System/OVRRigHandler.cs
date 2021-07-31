using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPIPlayerSpace;

public class OVRRigHandler : SingletonBehaviour<OVRRigHandler>
{
    [SerializeField] PlayerIKAnchor m_PlayerIKAnchor = default;
    public static PlayerIKAnchor PlayerIKAnchor => _Singleton.m_PlayerIKAnchor;
}
