using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : CameraBillboard
{
    [SerializeField] Transform m_LookAnchor = default;
    private void Awake()
    {
        m_TargetAnchor = m_LookAnchor;
    }
}
