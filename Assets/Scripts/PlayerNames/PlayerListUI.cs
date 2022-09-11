using System;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace PlayerNames
{
    /// <summary>
    /// Controls list of player names
    /// </summary>
    public class PlayerListUI : NetworkBehaviour
    {
        [SerializeField] private GameObject playerListItemPrefab;

        private void Awake()
        {
            PlayerNameTracker.OnNameChange += OnNameChanged;
        }

        private void UpdatePlayerList()
        {
            var playerList = PlayerNameTracker.GetPlayerNameList();

            for (int i = transform.childCount; i < playerList.Count; i++)
            {
                Instantiate(playerListItemPrefab, transform);
                Debug.Log("adding item");
            }
            for (int i = transform.childCount - 1; i >= playerList.Count; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
                Debug.Log("removing item");
            }

            for (int i = 0; i < playerList.Count; i++)
            {
                Debug.Log("setting up item " + i);
                transform.GetChild(i).GetComponent<PlayerListItemUI>().SetPlayerName(playerList[i]);
            }
        }

        private void OnNameChanged(NetworkConnection conn, string playerName)
        {
            UpdatePlayerList();
        }
    }
}
