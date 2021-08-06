using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using Prefab;

public class PokerBoard : BaseItem
{
    [SerializeField] ToggleEventHandler m_GrabToggleButton = default;
    [SerializeField] ButtonEventHandler m_StabilizeButton = default;
    NetworkVariableBool m_IsLockedNV = new NetworkVariableBool(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.Everyone });
    bool IsLocked { set { m_IsLockedNV.Value = value; } get { return m_IsLockedNV.Value; } }

    override protected void Awake()
    {
        m_IsLockedNV.OnValueChanged += (pre, cur) => LockGrab(cur);
        base.Awake();
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        m_GrabToggleButton.SetButtonEvent(isPressed => IsLocked = isPressed);
        m_StabilizeButton.SetPressEvent(StabilizeServerRpc);
    }
    [ServerRpc(RequireOwnership = false)]
    void StabilizeServerRpc()
    {
        var angles = transform.eulerAngles;
        transform.eulerAngles = new Vector3(0, angles.y, 0);
    }
    public void LockGrab(bool isLocked)
    {
        m_GrabTarget.IsTargettable = !isLocked;
        m_GrabToggleButton.Set(isLocked);
    }

    public static void Generate(Vector3 position, Quaternion rotation = default)
    => PrefabGenerator.SpawnNetworkPrefab(NetworkPrefabName.PokerBoard, position);
}
