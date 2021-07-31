using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public interface INetworkHandler
{
    NetworkBehaviour FindNetworkBehaviour(ushort behaviourId);
}
public interface IServerRpc
{
    
}