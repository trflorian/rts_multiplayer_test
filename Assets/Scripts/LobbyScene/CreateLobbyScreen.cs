using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LobbyScene
{
    public class CreateLobbyScreen : MonoBehaviour {
        [SerializeField] private TMP_InputField nameInput, maxPlayersInput;
        [SerializeField] private Button createButton;

        private void Start()
        {
            nameInput.onValueChanged.AddListener(CheckLobbyName);
            CheckLobbyName(nameInput.text);
        }

        private void CheckLobbyName(string lobbyName)
        {
            createButton.interactable = lobbyName.Length > 0;
        }

        public static event Action<LobbyData> LobbyCreated;

        public void OnCreateClicked() {
            var lobbyData = new LobbyData {
                Name = nameInput.text,
                MaxPlayers = int.Parse(maxPlayersInput.text),
            };

            LobbyCreated?.Invoke(lobbyData);
        }
    }
}