using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetworkInitializable
{
    void NetworkInit(int[] infos);
}