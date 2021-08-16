using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Spawning;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using UniRx;
using Prefab;
using MyFunc;

namespace GunSpace
{
    abstract public class BaseGun : BaseItem, IControllable
    {
        [SerializeField] protected GunInfo m_GunInfo;
        [SerializeField] protected BulletInfo m_BulletInfo;

        abstract protected NetworkPrefabName BulletName { get; }
        [SerializeField] protected Transform m_MuzzleAnchor = default;
        [SerializeField] protected ReloadEventHandler m_ReloadEventHandler = default;
        protected NetworkVariableInt m_BulletCountNV = new NetworkVariableInt();
        protected int BulletCount { set { m_BulletCountNV.Value = value; } get { return m_BulletCountNV.Value; } }
        abstract protected bool IsShootable { get; }
        abstract protected bool IsReloadable { get; }

        [SerializeField] bool m_Infinite = true;

        override public void OnSpawn()
        {
            base.OnSpawn();
            BulletCount = m_GunInfo.MaxBullets;
            m_ReloadEventHandler.SetInteractEvent((info =>
            {
                var action = info.ActionInfo;
                var interactor = info.Interactor;
                switch (action)
                {
                    case ReloadAction reloadAction:
                        if (interactor is BaseAmmo ammo)
                            ReloadServerRpc(reloadAction.Count, ammo.NetworkObjectId);
                        break;
                }
            }));
        }
        override public void OnPool()
        {
            base.OnPool();
        }
        [ServerRpc(RequireOwnership = false)]
        protected void ReloadServerRpc(int count, ulong interactorId)
        {
            if (IsReloadable)
            {
                var current = BulletCount;
                BulletCount = Mathf.Clamp(current + count, 0, m_GunInfo.MaxBullets);
                var interactor = NetworkSpawnManager.SpawnedObjects[interactorId];
                PrefabGenerator.DespawnPrefabOnServer(interactor);
                if (!IsHost)
                    OnReload();
                OnReloadClientRpc();
            }
        }
        [ClientRpc]
        void OnReloadClientRpc() => OnReload();
        virtual protected void OnReload() { }
        [ServerRpc(RequireOwnership = false)]
        virtual protected void ShotServerRpc()
        {
            if (IsShootable)
            {
                if (!m_Infinite)
                    BulletCount--;
                SpawnBullet();
                if (!IsHost)
                    OnShot();
                OnShotClientRpc();
            }
        }
        [ClientRpc]
        void OnShotClientRpc() => OnShot();
        virtual protected void OnShot() { }
        virtual protected void SpawnBullet()
        {
            var obj = PrefabGenerator.SpawnPrefabOnServer(BulletName, OwnerClientId, m_MuzzleAnchor.position, m_MuzzleAnchor.rotation);
            if (obj.TryGetComponent<BaseBullet>(out var bullet))
            {
                if (!IsHost)
                    bullet.Set(m_BulletInfo.Velocity, m_BulletInfo.Lifetime);
                bullet.SetClientRpc(m_BulletInfo.Velocity, m_BulletInfo.Lifetime);
            }
        }
        virtual public void Connect(InputManager.HandInput input)
        {
            input.IndexPress.AddListener(pressed => { if (pressed && IsShootable) ShotServerRpc(); });
        }
    }
    [Serializable]
    public struct GunInfo
    {
        public int MaxBullets;
        public GunInfo(int max)
        {
            MaxBullets = max;
        }
    }
}