using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using Cysharp.Threading.Tasks;

namespace GunSpace
{
    public class BaseBullet : NetworkPoolableParent, INetworkInitializable, IDamageSource
    {
        TrailLaser m_TrailLaser;
        float m_Velocity;
        float m_DamageValue;
        RaycastHit[] m_RaycastResults;
        CancellationTokenSource m_DespawnCTS;

        override protected void Awake()
        {
            base.Awake();
            m_TrailLaser = GetComponent<TrailLaser>();
            m_RaycastResults = new RaycastHit[8];
        }
        override public void OnPool()
        {
            m_DespawnCTS?.Cancel();
            base.OnPool();
        }
        private void Update()
        {
            m_TrailLaser.AddPosition(transform.position);
            if (IsOwner)
            {
                var length = Physics.RaycastNonAlloc(transform.position, transform.forward, m_RaycastResults, m_Velocity, (int)LayerName.RaycastTarget, QueryTriggerInteraction.Ignore);
                for (var i = 0; i < length; i++)
                {
                    m_RaycastResults[i].collider?.GetComponent<IEventReceivable<DamageEvent>>()?.SendEvent(new DamageEvent(this, m_DamageValue));
                }
            }
            transform.position += transform.forward * m_Velocity * Time.deltaTime;
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