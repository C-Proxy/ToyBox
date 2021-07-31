using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using System.Linq;
using UniRx;
using MyFunc;
using Prefab;
using PlayingCardSpace;

public class PlayingCardStacker : StackParentBehaviour<PlayingCardStacker, PlayingCard, CardInfo>, IControllable, IStackRpcCaller
{
    override protected LocalPrefabName ChildPrefabName => LocalPrefabName.PlayingCard;
    override protected float CHILD_MASS => 0.0005f;
    const float SPREAD_ANGLE = 90f;
    const float SPREAD_POSITION = 0.2f;
    const float THICKNESS = 0.0005f;
    NetworkVariableBool m_IsDeckNV;
    public bool IsDeck { private set { m_IsDeckNV.Value = value; } get { return m_IsDeckNV.Value; } }

    override public void OnSpawn()
    {
        base.OnSpawn();
        m_IsDeckNV = new NetworkVariableBool(new NetworkVariableSettings() { WritePermission = NetworkVariablePermission.Everyone });
        m_IsDeckNV.OnValueChanged += (pre, cur) => Align();
    }
    override public void OnPool()
    {
        m_IsDeckNV = null;
        base.OnPool();
    }

    [ServerRpc(RequireOwnership = false)]
    public void HandoverServerRpc(ulong receiverId, int index, int count)
    => Handover(NetworkFunc.GetComponent<PlayingCardStacker>(receiverId), index, count);
    [ServerRpc(RequireOwnership = false)]
    public void HandoverToGrabberServerRpc(NetworkInfo networkInfo, int index, int count)
    {
        var receiver = PrefabGenerator.SpawnPrefabWithoutManager(PrefabHash).GetComponent<PlayingCardStacker>();
        receiver.RequestChangeParent(networkInfo.ToComponent<IGrabber>());
        Handover(receiver, index, count);
    }

    public void SetFullDeck()
    => ChildInfos = Enumerable.Range(0, 53).Select(count => new CardInfo((byte)count)).ToArray();
    void SetDeckMode(bool enable) => IsDeck = enable;
    [ServerRpc(RequireOwnership = false)]
    void ShuffleServerRpc()
    {
        var infos = ChildInfos.ToList();
        infos.Suffle(DateTime.Now.Millisecond);
        ChildInfos = infos.ToArray();
    }
    override protected void Align()
    {
        var cnt = m_ChildList.Count;
        if (IsDeck)
        {
            float pos = 0f;
            foreach (var card in m_ChildList)
            {
                card.transform.localPosition = new Vector3(0, pos, 0);
                card.transform.localRotation = default;
                pos += THICKNESS;
            }
        }
        else
        {
            var log = Mathf.Log(cnt, 53);
            var angleSpread = log * SPREAD_ANGLE;
            var angle = -angleSpread / 2;
            var deltaAngle = angleSpread / cnt;
            var posSpread = log * SPREAD_POSITION;
            var pos = new Vector3(-posSpread / 2, 0, 0.01f);
            var deltaPos = new Vector3(posSpread / cnt, THICKNESS, 0);
            foreach (var card in m_ChildList)
            {
                card.transform.localEulerAngles = new Vector3(0, angle, 0);
                card.transform.localPosition = pos;
                angle += deltaAngle;
                pos += deltaPos;
            }
        }
    }

    public IDisposable[] Connect(InputDriver.PlayerInput playerInput)
    {
        return new[]{
        playerInput.ClickAsObservable(KeyType.MainButton).Where(isDouble=>!isDouble).Subscribe(_=>SetDeckMode(!IsDeck)),
        playerInput.ClickAsObservable(KeyType.SubButton).Where(isDouble=>!isDouble).Subscribe(_=>ShuffleServerRpc()),
        };

    }
    override public void OnGrab(IGrabber parent)
    {
        base.OnGrab(parent);
    }

    public static void GenerateFullDeck(Vector3 position = default, Quaternion rotation = default)
    => PrefabGenerator.SpawnNetworkPrefab(NetworkPrefabName.PlayingCardStacker, position, rotation);

    override public string ToString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(base.ToString());
        stringBuilder.Append("\nIsDeck:");
        stringBuilder.Append(IsDeck);
        stringBuilder.Append("\nIsGrabbed");
        stringBuilder.Append(IsGrabbed);
        return stringBuilder.ToString();
    }
    [ContextMenu("ToString()")]
    void Display() => Debug.Log(ToString());
}