using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace PBS.Networking
{
    public class ManagerHUD : NetworkManagerHUD
    {
        public bool isServer = true;
        Networking.Manager networkManager;

        void Awake()
        {
            networkManager = GetComponent<Networking.Manager>();
        }

        private void Start()
        {
            if (isServer)
            {
                networkManager.StartServer();
            }
        }

        void OnGUI()
        {
            if (!showGUI)
                return;

            GUILayout.BeginArea(new Rect(offsetX, offsetY, 120, 120));
            if (!NetworkClient.isConnected && !NetworkServer.active && !isServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
            }

            // client ready
            if (!isServer)
            {
                if (NetworkClient.isConnected && !ClientScene.ready)
                {
                    if (GUILayout.Button("Client Ready"))
                    {
                        ClientScene.Ready(NetworkClient.connection);

                        if (ClientScene.localPlayer == null)
                        {
                            ClientScene.AddPlayer(NetworkClient.connection);
                        }
                    }
                }
                StopButtons();
            }
            GUILayout.EndArea();
        }

        void StartButtons()
        {
            if (!NetworkClient.active)
            {
                // Server + Client
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    if (GUILayout.Button("Host + Client"))
                    {
                        networkManager.StartHost();
                    }
                }

                // Client + IP
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Client"))
                {
                    networkManager.StartClient();
                }
                networkManager.networkAddress = GUILayout.TextField(networkManager.networkAddress);
                GUILayout.EndHorizontal();

                // Server Only
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    // cant be a server in webgl build
                    GUILayout.Box("(  WebGL cannot be server  )");
                }
                else
                {
                    if (GUILayout.Button("Server Only")) networkManager.StartServer();
                }
            }
            else
            {
                // Connecting
                GUILayout.Label("Connecting to " + networkManager.networkAddress + "..");
                if (GUILayout.Button("Cancel Connection Attempt"))
                {
                    networkManager.StopClient();
                }
            }
        }

        void StatusLabels()
        {
            // server / client status message
            if (NetworkServer.active)
            {
                GUILayout.Label("Server: active. Transport: " + Transport.activeTransport);
            }
            if (NetworkClient.isConnected)
            {
                GUILayout.Label("Client: address=" + networkManager.networkAddress);
            }
        }

        void StopButtons()
        {
            // stop host if host mode
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Host"))
                {
                    networkManager.StopHost();
                }
            }
            // stop client if client-only
            else if (NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Client"))
                {
                    networkManager.StopClient();
                }
            }
            // stop server if server-only
            else if (NetworkServer.active)
            {
                if (GUILayout.Button("Stop Server"))
                {
                    networkManager.StopServer();
                }
            }
        }
    }
}

