using System;
using PlayerNames;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MainMenu
{
    /// <summary>
    /// Input field for entering the player name
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        public const string PlayerNamePrefKey = "player_name";

        private TMP_InputField _inputField;

        private void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
            _inputField.onValueChanged.AddListener(OnPlayerNameChanged);
        }

        private void Start()
        {
            string playerName;
            if (PlayerPrefs.HasKey(PlayerNamePrefKey))
            {
                playerName = PlayerPrefs.GetString(PlayerNamePrefKey);
            }
            else
            {
                playerName = GenerateRandomPlayerName();
                SavePlayerName(playerName);
            }
            _inputField.text = playerName;
        }

        private void OnPlayerNameChanged(string newPlayerName)
        {
            SavePlayerName(newPlayerName);
        }

        private void SavePlayerName(string newPlayerName)
        {
            PlayerPrefs.SetString(PlayerNamePrefKey, newPlayerName);
            PlayerPrefs.Save();
        }

        private string GenerateRandomPlayerName()
        {
            return RandomNames[Random.Range(0, RandomNames.Length - 1)] + Random.Range(1000, 9999);
        }

        private static readonly string[] RandomNames =
        {
            "Aiden",
            "Blake",
            "Briar",
            "Campbell",
            "Echo",
            "Eden",
            "Ellis",
            "Gavi",
            "Haven",
            "Hayden",
            "Indigo",
            "Ocean",
            "Onyx",
            "Peyton",
            "Phoenix",
            "Quinn",
            "Reese",
            "Reign",
            "Riley",
            "River",
            "Royal",
            "Rory",
            "Rylan",
            "Sage",
            "Sawyer",
            "Shay",
            "Shiloh",
            "Sky",
            "Skyler",
            "Sterling",
            "Storm",
            "Story",
            "Taylen",
            "Teagan",
            "Tristan",
            "Tru",
            "Vesper",
            "Winter",
            "Woods",
            "Wren",
            "Zen",
            "Zion",
        };
    }
}