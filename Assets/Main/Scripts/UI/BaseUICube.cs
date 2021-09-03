using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MaterialSpace;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

[RequireComponent(typeof(UICube))]
abstract public class BaseUICube : BaseItem, IControllable
{
    const float LERP_TIME = .5f;
    protected const float DEFAULT_SIZE = 0.02f;
    UICube m_UICube;
    public bool IsLerping => m_LerpCTS != null;

    NetworkVariableVector3 m_ScaleNV = new NetworkVariableVector3(new Vector3(DEFAULT_SIZE, DEFAULT_SIZE, DEFAULT_SIZE));
    public Vector3 CubeScale { set { m_ScaleNV.Value = value; } get { return m_ScaleNV.Value; } }
    virtual protected void OnScaleChanged(Vector3 pre, Vector3 cur)
    {
        m_UICube.SetScale(cur);
    }
    CancellationTokenSource m_LerpCTS;

    override protected void Awake()
    {
        base.Awake();
        m_UICube = GetComponent<UICube>();
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        m_ScaleNV.OnValueChanged += OnScaleChanged;
    }
    override public void OnPool()
    {
        m_ScaleNV.OnValueChanged -= OnScaleChanged;
        base.OnPool();
    }
    public void Connect(InputManager.HandInput input)
    {
        // input.IndexPress.AddListener(pressed =>
        // {
        //     LerpScaleServerRpc(new Vector3(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f)));
        // });
    }

    virtual protected void SetDefault()
    {
        m_UICube.SetScale(new Vector3(DEFAULT_SIZE, DEFAULT_SIZE, DEFAULT_SIZE));
        m_UICube.SetMaterial(MaterialName.White);
    }
    [ServerRpc]
    protected void SetScaleServerRpc(Vector3 scale)
    => CubeScale = scale;
    [ServerRpc]
    protected void LerpScaleServerRpc(Vector3 scale)
    {
        LerpScaleAsync(scale).Forget();
    }
    async protected UniTask LerpScaleAsync(Vector3 scale)
    {
        if (m_LerpCTS != null) m_LerpCTS.Cancel();
        m_LerpCTS = new CancellationTokenSource();
        var preScale = CubeScale;
        var scaleEnumerable = UniTaskAsyncEnumerable.Create<Vector3>(async (writer, token) =>
        {
            float sumTime = 0, ratio;
            while (!token.IsCancellationRequested)
            {
                sumTime += Time.deltaTime;
                ratio = sumTime / LERP_TIME;
                if (ratio > 1f)
                    break;
                await writer.YieldAsync(Vector3.Lerp(preScale, scale, ratio));
                await UniTask.Yield();
            }
            await writer.YieldAsync(scale);
        });
        try
        {
            await foreach (var lerpedScale in scaleEnumerable.WithCancellation(m_LerpCTS.Token))
            {
                CubeScale = lerpedScale;
            }
        }
        catch (OperationCanceledException) { throw; }
        m_LerpCTS = null;
    }
    async public UniTask CloseAsync()
    {
        await LerpScaleAsync(new Vector3(DEFAULT_SIZE, DEFAULT_SIZE, DEFAULT_SIZE));
        PrefabGenerator.DespawnPrefabOnServer(NetworkObject);

    }
}
