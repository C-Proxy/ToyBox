using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Prefab;
public class BurstEffect : LocalPoolableBehaviour
{
    override public LocalPrefabName PrefabName => LocalPrefabName.Eff_Burst;
    [SerializeField] ParticleSystem m_Cracker = default, m_Smoke = default;
    override public void OnSpawn()
    {
        m_Cracker.Emit(30);
        m_Smoke.Emit(20);
        DespawnAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }
    async UniTaskVoid DespawnAsync(CancellationToken token)
    {
        await UniTask.WaitWhile(() => m_Cracker.IsAlive(false) && m_Smoke.IsAlive(false));
        PrefabGenerator.PoolLocalObject(this);
    }
}
