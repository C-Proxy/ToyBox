using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using Cysharp.Threading.Tasks;
using TMPro;
using Prefab;
using GunSpace;

public class Wingman : BaseGun
{
    override protected NetworkPrefabName BulletName => NetworkPrefabName.NormalBullet;
    [SerializeField] ParticleSystem m_MuzzleFlash = default, m_Smoke = default;
    [SerializeField] Transform m_CyllinderAnchor = default;
    TextMeshPro m_BulletCountTMP;
    Animator m_Animator;
    override protected bool IsShootable { get { return !IsOpen && m_MagazineMock && BulletCount > 0; } }
    override protected bool IsReloadable { get { return IsOpen && m_MagazineMock == null; } }
    NetworkVariableBool m_IsOpenNV;
    bool IsOpen { set { m_IsOpenNV.Value = value; } get { return m_IsOpenNV.Value; } }

    override protected void Awake()
    {
        m_BulletCountTMP = GetComponentInChildren<TextMeshPro>();
        m_Animator = GetComponent<Animator>();
        base.Awake();
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        m_BulletCountNV.OnValueChanged += (pre, cur) => m_BulletCountTMP.text = cur.ToString();
        m_BulletCountTMP.text = m_GunInfo.MaxBullets.ToString();

        m_IsOpenNV = new NetworkVariableBool(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.Everyone });
        m_IsOpenNV.OnValueChanged += (pre, cur) =>
        {
            m_Animator.SetBool("IsOpen", cur);
            m_ReloadEventHandler.IsTargettable = cur;
            if (cur)
                Eject();
        };
    }
    override public void OnPool()
    {
        m_BulletCountNV = null;
        base.OnPool();
    }
    [ClientRpc]
    override protected void OnReloadClientRpc()
    {
        var mock = PrefabGenerator.GenerateLocalPrefab(LocalPrefabName.Mock_WingmanAmmo, m_CyllinderAnchor.position, m_CyllinderAnchor.rotation).GetComponent<MockObject>();
        m_MagazineMock = mock;
        mock.transform.SetParent(m_CyllinderAnchor);
        mock.Rigidbody.isKinematic = true;
    }
    [ClientRpc]
    override protected void OnShotClientRpc()
    {
        m_MuzzleFlash.Emit(6);
        m_Smoke.Emit(3);
    }
    [SerializeField] MockObject m_MagazineMock;
    void Eject()
    {
        if (m_MagazineMock)
        {
            var mock = m_MagazineMock;
            mock.transform.SetParent(null);
            var rigidbody = mock.Rigidbody;
            rigidbody.isKinematic = false;
            rigidbody.AddForce(-transform.right);
            UniTask.Run(async () =>
            {
                await UniTask.Delay(TimeSpan.FromSeconds(3f));
                PrefabGenerator.PoolLocalObject(mock);
            });
        }
    }

    override public void Connect(InputManager.HandInput input)
    {
        base.Connect(input);
        input.MainClick.AddListener(isDouble => { if (!isDouble) IsOpen = !IsOpen; });
    }

    public static void Generate(Vector3 position = default, Quaternion rotation = default)
    => PrefabGenerator.SpawnNetworkPrefab(NetworkPrefabName.Wingman, position, rotation);
}
