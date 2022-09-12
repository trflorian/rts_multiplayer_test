using Managers;
using TMPro;
using UnityEngine;

namespace LobbyScene
{
    public class LobbyPlayerPanel : MonoBehaviour {
        [SerializeField] private TMP_Text nameText, statusText;

        public ulong PlayerId { get; private set; }

        public void Init(ulong playerId, PlayerData playerData) {
            PlayerId = playerId;
            
            SetPlayerData(playerData);
        }

        public void SetPlayerData(PlayerData playerData)
        {
            nameText.text = playerData.PlayerName;
            statusText.text = playerData.IsReady ? "Ready" : "Not Ready";
            statusText.color = playerData.IsReady ? Color.green : Color.white;
        }
    }
}