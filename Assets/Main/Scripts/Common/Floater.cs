using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField] float m_FloatHight = 1f;
    Transform m_ParentAnchor;
    private void Awake()
    {
        m_ParentAnchor = transform.parent;
    }
    private void Update()
    {
        transform.position = m_ParentAnchor.position + new Vector3(0, m_FloatHight, 0);
        transform.rotation = Quaternion.identity;
    }
}
