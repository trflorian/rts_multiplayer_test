using TMPro;
using UnityEngine;

namespace LobbyScene
{
    public class LobbyPlayerPanel : MonoBehaviour {
        [SerializeField] private TMP_Text nameText, statusText;

        public ulong PlayerId { get; private set; }

        public void Init(ulong playerId) {
            PlayerId = playerId;
            nameText.text = $"Player {playerId}";
        }

        public void SetReady(bool isReady) {
            statusText.text = isReady ? "Ready" : "Not Ready";
            statusText.color = isReady ? Color.green : Color.white;
        }
    }
}