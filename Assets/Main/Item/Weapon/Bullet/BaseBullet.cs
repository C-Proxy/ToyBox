using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using Cysharp.Threading.Tasks;

namespace GunSpace
{
    public class BaseBullet : PoolableNetworkBehaviour, INetworkInitializable
    {
        float m_Velocity;
        CancellationTokenSource m_DespawnCTS;

        override public void OnSpawn() { }
        override public void OnPool()
        {
            m_DespawnCTS?.Cancel();
            base.OnPool();
        }
        private void Update()
        {
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
            await UniTask.Delay(TimeSpan.FromSeconds(lifetime), DelayType.DeltaTime, PlayerLoopTiming.Update, token);
            DespawnServerRpc();
        }
    }

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