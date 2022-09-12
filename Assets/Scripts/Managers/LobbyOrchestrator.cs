using System;
using System.Collections.Generic;
using LobbyScene;
using Misc;
using Services;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
#pragma warning disable CS4014

    /// <summary>
    /// Lobby orchestrator. I put as much UI logic within the three sub screens,
    /// but the transport and RPC logic remains here. It's possible we could pull
    /// </summary>
    public class LobbyOrchestrator : NetworkBehaviour {
        [SerializeField] private MainLobbyScreen mainLobbyScreen;
        [SerializeField] private CreateLobbyScreen createScreen;
        [SerializeField] private RoomScreen roomScreen;

        private void Start() {
            mainLobbyScreen.gameObject.SetActive(true);
            createScreen.gameObject.SetActive(false);
            roomScreen.gameObject.SetActive(false);

            CreateLobbyScreen.LobbyCreated += CreateLobby;
            LobbyRoomPanel.LobbySelected += OnLobbySelected;
            RoomScreen.LobbyLeft += OnLobbyLeft;
            RoomScreen.StartPressed += OnGameStart;
        
            NetworkObject.DestroyWithScene = true;
        }

        #region Main Lobby

        private async void OnLobbySelected(Unity.Services.Lobbies.Models.Lobby lobby) {
            using (new Load("Joining Lobby...")) {
                try {
                    await MatchmakingService.JoinLobbyWithAllocation(lobby.Id);

                    mainLobbyScreen.gameObject.SetActive(false);
                    roomScreen.gameObject.SetActive(true);

                    NetworkManager.Singleton.StartClient();
                }
                catch (Exception e) {
                    Debug.LogError(e);
                    CanvasUtilities.Instance.ShowError("Failed joining lobby");
                }
            }
        }

 

        #endregion

        #region Create

        private async void CreateLobby(LobbyData data) {
            using (new Load("Creating Lobby...")) {
                try {
                    await MatchmakingService.CreateLobbyWithAllocation(data);

                    createScreen.gameObject.SetActive(false);
                    roomScreen.gameObject.SetActive(true);

                    // Starting the host immediately will keep the relay server alive
                    NetworkManager.Singleton.StartHost();
                }
                catch (Exception e) {
                    Debug.LogError(e);
                    CanvasUtilities.Instance.ShowError("Failed creating lobby");
                }
            }
        }

        #endregion

        #region Room

        private readonly Dictionary<ulong, bool> _playersInLobby = new();
        public static event Action<Dictionary<ulong, bool>> LobbyPlayersUpdated;
        private float _nextLobbyUpdate;

        public override void OnNetworkSpawn() {
            if (IsServer) {
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
                _playersInLobby.Add(NetworkManager.Singleton.LocalClientId, false);
                UpdateInterface();
            }

            // Client uses this in case host destroys the lobby
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;

 
        }

        private void OnClientConnectedCallback(ulong playerId) {
            if (!IsServer) return;

            // Add locally
            if (!_playersInLobby.ContainsKey(playerId) && playerId != NetworkManager.Singleton.LocalClientId) _playersInLobby.Add(playerId, false);

            PropagateToClients();

            UpdateInterface();
        }

        private void PropagateToClients() {
            foreach (var player in _playersInLobby) UpdatePlayerClientRpc(player.Key, player.Value);
        }

        [ClientRpc]
        private void UpdatePlayerClientRpc(ulong clientId, bool isReady) {
            if (IsServer) return;

            if (!_playersInLobby.ContainsKey(clientId)) _playersInLobby.Add(clientId, isReady);
            else _playersInLobby[clientId] = isReady;
            UpdateInterface();
        }

        private void OnClientDisconnectCallback(ulong playerId) {
            if (IsServer) {
                // Handle locally
                if (_playersInLobby.ContainsKey(playerId)) _playersInLobby.Remove(playerId);

                // Propagate all clients
                RemovePlayerClientRpc(playerId);

                UpdateInterface();
            }
            else {
                // This happens when the host disconnects the lobby
                roomScreen.gameObject.SetActive(false);
                mainLobbyScreen.gameObject.SetActive(true);
                OnLobbyLeft();
            }
        }

        [ClientRpc]
        private void RemovePlayerClientRpc(ulong clientId) {
            if (IsServer) return;

            if (_playersInLobby.ContainsKey(clientId)) _playersInLobby.Remove(clientId);
            UpdateInterface();
        }

        public void OnReadyClicked() {
            SetReadyServerRpc(NetworkManager.Singleton.LocalClientId);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetReadyServerRpc(ulong playerId) {
            _playersInLobby[playerId] = !_playersInLobby[playerId];
            PropagateToClients();
            UpdateInterface();
        }

        private void UpdateInterface() {
            LobbyPlayersUpdated?.Invoke(_playersInLobby);
        }

        private async void OnLobbyLeft() {
            using (new Load("Leaving Lobby...")) {
                _playersInLobby.Clear();
                NetworkManager.Singleton.Shutdown();
                await MatchmakingService.LeaveLobby();
            }
        }
    
        public override void OnDestroy() {
     
            base.OnDestroy();
            CreateLobbyScreen.LobbyCreated -= CreateLobby;
            LobbyRoomPanel.LobbySelected -= OnLobbySelected;
            RoomScreen.LobbyLeft -= OnLobbyLeft;
            RoomScreen.StartPressed -= OnGameStart;
        
            // We only care about this during lobby
            if (NetworkManager.Singleton != null) {
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
            }
      
        }
    
        private async void OnGameStart() {
            using (new Load("Starting the game...")) {
                await MatchmakingService.LockLobby();
                NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
            }
        }

        public void ClickQuit()
        {
            AuthenticationService.Instance.SignOut();
            SceneManager.LoadScene("MainMenuScene");
        }

        #endregion
    }

    public struct LobbyData {
        public string Name;
        public int MaxPlayers;
    }
}