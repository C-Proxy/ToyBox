using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBillboard : MonoBehaviour
{
    protected Transform m_TargetAnchor;
    [SerializeField] bool m_UseWorldUpward;
    private void Awake()
    {
        m_TargetAnchor = Camera.main.transform;
    }

    private void Update()
    {
        if (m_UseWorldUpward)
            transform.LookAt(m_TargetAnchor, Vector3.up);
        else
            transform.LookAt(m_TargetAnchor, m_TargetAnchor.up);
    }
}
