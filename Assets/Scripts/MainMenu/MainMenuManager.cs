using System;
using FishNet;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Transporting;
using FishNet.Utility;
using UnityEngine;

namespace MainMenu
{
    /// <summary>
    /// Manages network setup in main menu
    /// </summary>
    public class MainMenuManager : NetworkBehaviour
    {
        private NetworkManager _networkManager;

        private void Awake()
        {
            _networkManager = InstanceFinder.NetworkManager;
        }

        private void StartClient()
        {
            _networkManager.ClientManager.StartConnection();
        }

        private void StartServer()
        {
            _networkManager.ServerManager.StartConnection();
        }

        public void OnClickStartHost()
        {
            StartServer();
            StartClient();
        }

        public void OnClickStartClient()
        {
            StartClient();
        }
    }
}
