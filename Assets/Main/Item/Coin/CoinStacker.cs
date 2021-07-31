using UnityEngine;
using MLAPI.Messaging;
using System.Linq;
using MyFunc;
using Prefab;
using CoinSpace;

public class CoinStacker : StackParentBehaviour<CoinStacker, Coin, CoinInfo>
{
    const float THICKNESS = 0.0045f;
    override protected LocalPrefabName ChildPrefabName => LocalPrefabName.Coin;
    override protected float CHILD_MASS => 0.005f;

    public void Set(Coinage coinage, int count)
    => ChildInfos = Enumerable.Range(0, count).Select(_ => new CoinInfo(coinage)).ToArray();

    override protected void Align()
    {
        float pos = 0f;
        foreach (var coin in m_ChildList)
        {
            coin.transform.localPosition = new Vector3(0, pos, 0);
            coin.transform.localRotation = default;
            pos += THICKNESS;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void HandoverServerRpc(ulong receiverId, int index, int count)
    => Handover(NetworkFunc.GetComponent<CoinStacker>(receiverId), index, count);
    [ServerRpc(RequireOwnership = false)]
    public void HandoverToGrabberServerRpc(NetworkInfo networkInfo, int index, int count)
    {
        var receiver = PrefabGenerator.SpawnPrefabWithoutManager(PrefabHash).GetComponent<CoinStacker>();
        receiver.RequestChangeParent(networkInfo.ToComponent<IGrabber>());
        Handover(receiver, index, count);
    }

    public int GetSum(Coin target)
    {
        var index = m_ChildList.IndexOf(target);
        return m_ChildList.GetRange(index, m_ChildList.Count - index).Select(coin => coin.Value).Aggregate((sum, next) => sum + next);
    }

    public static void GenerateCoins(Vector3 position, Quaternion rotation, Coinage coinage, int count)
    => PrefabGenerator.SpawnNetworkPrefab(NetworkPrefabName.CoinStacker, position, rotation, new[] { (int)coinage, count });

    override public string ToString() => m_ChildList.ToString();
}