using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using Services;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace LobbyScene
{
    /// <summary>
    ///     NetworkBehaviours cannot easily be parented, so the network logic will take place
    ///     on the network scene object "NetworkLobby"
    /// </summary>
    public class RoomScreen : MonoBehaviour {
        [SerializeField] private LobbyPlayerPanel playerPanelPrefab;
        [SerializeField] private Transform playerPanelParent;
        [SerializeField] private TMP_Text waitingText;
        [SerializeField] private GameObject startButton, readyButton;

        private readonly List<LobbyPlayerPanel> _playerPanels = new();
        private bool _allReady;
        private bool _ready;

        public static event Action StartPressed; 

        private void OnEnable() {
            foreach (Transform child in playerPanelParent) Destroy(child.gameObject);
            _playerPanels.Clear();

            LobbyOrchestrator.LobbyPlayersUpdated += NetworkLobbyPlayersUpdated;
            MatchmakingService.CurrentLobbyRefreshed += OnCurrentLobbyRefreshed;
            startButton.SetActive(false);
            readyButton.SetActive(false);

            _ready = false;
        }

        private void OnDisable() {
            LobbyOrchestrator.LobbyPlayersUpdated -= NetworkLobbyPlayersUpdated;
            MatchmakingService.CurrentLobbyRefreshed -= OnCurrentLobbyRefreshed;
        }

        public static event Action LobbyLeft;

        public void OnLeaveLobby() {
            LobbyLeft?.Invoke();
        }

        private void NetworkLobbyPlayersUpdated(Dictionary<ulong, PlayerData> players) {
            var allActivePlayerIds = players.Keys;

            // Remove all inactive panels
            var toDestroy = _playerPanels.Where(p => !allActivePlayerIds.Contains(p.PlayerId)).ToList();
            foreach (var panel in toDestroy) {
                _playerPanels.Remove(panel);
                Destroy(panel.gameObject);
            }

            foreach (var player in players) {
                var currentPanel = _playerPanels.FirstOrDefault(p => p.PlayerId == player.Key);
                if (currentPanel != null) { 
                    currentPanel.SetPlayerData(player.Value);
                }
                else {
                    var panel = Instantiate(playerPanelPrefab, playerPanelParent);
                    panel.Init(player.Key, player.Value);
                    _playerPanels.Add(panel);
                }
            }

            startButton.SetActive(NetworkManager.Singleton.IsHost && players.All(p => p.Value.IsReady));
            readyButton.SetActive(!_ready);
        }

        private void OnCurrentLobbyRefreshed(Lobby lobby) {
            waitingText.text = $"Waiting on players... {lobby.Players.Count}/{lobby.MaxPlayers}";
        }

        public void OnReadyClicked() {
            readyButton.SetActive(false);
            _ready = true;
        }

        public void OnStartClicked() {
            StartPressed?.Invoke();
        }
    }
}