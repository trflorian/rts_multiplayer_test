using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using Models;
using TMPro;
using UnityEngine;

namespace LobbyScene
{
    public class CreateLobbyScreen : MonoBehaviour {
        [SerializeField] private TMP_InputField nameInput, maxPlayersInput;

        private void Start() {
            void SetOptions(TMP_Dropdown dropdown, IEnumerable<string> values) {
                dropdown.options = values.Select(type => new TMP_Dropdown.OptionData { text = type }).ToList();
            }
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