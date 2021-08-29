using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : CameraBillboard
{
    [SerializeField] new protected Transform m_TargetAnchor = default;
}
