using Misc;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    /// <summary>
    /// Manages network setup in main menu
    /// </summary>
    public class MainMenuManager : MonoBehaviour
    {
        public async void LoginAnonymously() {
            using (new Load("Logging you in...")) {
                await Authentication.Login();
                SceneManager.LoadSceneAsync("LobbyScene");
            }
        }
    }
}
