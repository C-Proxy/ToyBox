using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class GameManager : MonoBehaviour
{
    [SerializeField] NetworkMode m_NetworkMode = default;
    private void Start()
    {
        var networkManager = NetworkManager.Singleton;
        switch (m_NetworkMode)
        {
            case NetworkMode.Server:
                networkManager.StartServer();
                break;
            case NetworkMode.Client:
                networkManager.StartClient();
                break;
            case NetworkMode.Host:
                networkManager.StartHost();
                break;
        }
    }
    enum NetworkMode
    {
        None,
        Server,
        Client,
        Host,
    }
}
