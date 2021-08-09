using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using MLAPI;
using MLAPI.Transports.UNET;

namespace Networking
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] NetworkMode m_DefaultMode = default;
        NetworkConfig m_NetworkConfig;
        private void Awake()
        {
            m_NetworkConfig = NetworkConfig.CreateFromFile("NetworkConfig.txt");
        }
        private void Start()
        {
            var networkManager = NetworkManager.Singleton;
            NetworkMode mode;
            if (m_NetworkConfig != null)
            {
                var unet = networkManager.GetComponent<UNetTransport>();
                unet.ConnectAddress = m_NetworkConfig.Address;
                unet.ConnectPort = m_NetworkConfig.ClientPort;
                unet.ServerListenPort = m_NetworkConfig.ServerPort;
                mode = m_NetworkConfig.NetworkMode;
            }
            else
            {
                mode = m_DefaultMode;
            }
            Debug.Log("Connect:" + mode.ToString());
            var vrEnable = !Application.isBatchMode;
            switch (mode)
            {
                case NetworkMode.Server:
                    networkManager.StartServer();
                    vrEnable = false;
                    break;
                case NetworkMode.Client:
                    networkManager.StartClient();
                    break;
                case NetworkMode.Host:
                    networkManager.StartHost();
                    break;
                default:
                    vrEnable = false;
                    break;
            }
            if (!vrEnable) DisableOculusVR();
        }
        private void DisableOculusVR()
        {
            var xrDisplays = new List<XRDisplaySubsystem>();
            SubsystemManager.GetInstances(xrDisplays);
            foreach (var display in xrDisplays)
            {
                if (display.subsystemDescriptor.id.Equals("oculus display"))
                {
                    display.Stop();
                    Debug.Log("Stopped Oculus VR.");
                    return;
                }
            }
        }
        public class NetworkConfig
        {
            static Dictionary<string, NetworkMode> m_ModeDictionary = new Dictionary<string, NetworkMode>() { { "Server", NetworkMode.Server }, { "Client", NetworkMode.Client }, { "Host", NetworkMode.Host } };
            public NetworkMode NetworkMode;
            public string Address;
            public int ClientPort;
            public int ServerPort;

            public NetworkConfig(NetworkMode mode, string address, int clientPort, int serverPort)
            {
                NetworkMode = mode;
                Address = address;
                ClientPort = clientPort;
                ServerPort = serverPort;
            }
            public static NetworkConfig CreateFromFile(string fileName)
            {
                var path = Application.dataPath + "/" + fileName;
                Debug.Log(path);
                if (File.Exists(path))
                {
                    using (var reader = new StreamReader(path, Encoding.GetEncoding("UTF-8")))
                    {
                        return new NetworkConfig(
                        m_ModeDictionary[reader.ReadLine().Trim()],
                        reader.ReadLine().Trim(),
                        int.Parse(reader.ReadLine().Trim()),
                        int.Parse(reader.ReadLine().Trim()));
                    }
                }
                else
                    return null;
            }
        }
        public enum NetworkMode
        {
            None,
            Server,
            Client,
            Host,
        }
    }
}
