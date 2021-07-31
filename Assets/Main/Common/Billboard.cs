using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    bool m_UseWorldUpward;
    Transform m_CameraAnchor;
    private void Awake()
    {
        m_CameraAnchor = Camera.main.transform;
    }
    private void Update()
    {
        if (m_UseWorldUpward)
            transform.LookAt(m_CameraAnchor, Vector3.up);
        else
            transform.LookAt(m_CameraAnchor, m_CameraAnchor.up);
    }
}
