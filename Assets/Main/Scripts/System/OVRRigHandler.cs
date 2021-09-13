using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRRigHandler : SingletonBehaviour<OVRRigHandler>
{
    const float AXIS_THRESHOLD = 0.5f * 0.5f;
    public static PlayerIKAnchor PlayerIKAnchor => _Singleton.m_PlayerIKAnchor;
    [SerializeField] PlayerIKAnchor m_PlayerIKAnchor = default;
    [SerializeField] float MoveSpeed = 1f;
    [SerializeField] float RotateAngle = 45f;
    public static OVRRigHandler Singleton => _Singleton;

    public void Move(Vector2 axis)
    {
        var moveVector = new Vector3(axis.x, 0, axis.y) * MoveSpeed * Time.deltaTime;
        transform.Translate(moveVector, Space.Self);
    }
    bool m_MoveCheck;
    public void Rotate(float angle)
    {
        if (angle * angle > AXIS_THRESHOLD)
        {
            if (!m_MoveCheck)
            {
                transform.Rotate(Vector3.up, Mathf.Sign(angle) * RotateAngle, Space.Self);
                m_MoveCheck = true;
            }
        }
        else if (m_MoveCheck)
            m_MoveCheck = false;
    }
}
