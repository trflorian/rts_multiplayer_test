using TMPro;
using UnityEngine;

namespace PlayerNames
{
    /// <summary>
    /// Item of player list
    /// </summary>
    public class PlayerListItemUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text playerNameText;
        
        public void SetPlayerName(string playerName)
        {
            playerNameText.SetText(playerName);
        }
    }
}