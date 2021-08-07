using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using Cysharp.Threading.Tasks;

namespace GunSpace
{
    public class BaseBullet : NetworkPoolableBehaviour, INetworkInitializable, IDamageDealer
    {
        TrailLaser m_TrailLaser;
        float m_Velocity;
        float m_DamageValue;
        RaycastHit[] m_RaycastResults;
        CancellationTokenSource m_DespawnCTS;

        override protected void Awake()
        {
            m_TrailLaser = GetComponent<TrailLaser>();
            m_TrailLaser.Init();
            m_RaycastResults = new RaycastHit[8];
            base.Awake();
        }
        override public void OnSpawn()
        {
            m_TrailLaser.OnSpawn();
        }
        override public void OnPool()
        {

            m_TrailLaser.OnPool();
            m_DespawnCTS?.Cancel();
            base.OnPool();
        }
        private void Update()
        {
            if (IsOwner)
            {
                var length = Physics.RaycastNonAlloc(transform.position, transform.forward, m_RaycastResults, m_Velocity, (int)LayerName.RaycastTarget, QueryTriggerInteraction.Ignore);
                for (var i = 0; i < length; i++)
                {
                    m_RaycastResults[i].collider?.GetComponent<ActionEventHandler>()?.Interact(this, new DamageAction(m_DamageValue));
                }
            }
            transform.position += transform.forward * m_Velocity * Time.deltaTime;
            m_TrailLaser.AddPosition(transform.position);
        }
        public void NetworkInit(RpcPackage package)
        {
            SetClientRpc(package.GetFloat(), package.GetFloat());
        }
        public void Set(float velocity, float lifetime)
        {
            m_Velocity = velocity;
            if (IsOwner)
            {
                m_DespawnCTS = new CancellationTokenSource();
                DespawnAsync(lifetime, m_DespawnCTS.Token).Forget();
            }
        }
        [ClientRpc]
        public void SetClientRpc(float velocity, float lifetime)
        => Set(velocity, lifetime);
        async UniTaskVoid DespawnAsync(float lifetime, CancellationToken token)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(lifetime), DelayType.DeltaTime, PlayerLoopTiming.Update, token);
            }
            catch { }
            DespawnServerRpc();
        }

        public void OnDealDamage()
        {
            m_DespawnCTS.Cancel();
            DespawnServerRpc();
        }
    }
    [Serializable]
    public struct BulletInfo
    {
        public float Velocity;
        public float Lifetime;
        public BulletInfo(float velocity, float lifetime)
        {
            Velocity = velocity;
            Lifetime = lifetime;
        }
    }
}