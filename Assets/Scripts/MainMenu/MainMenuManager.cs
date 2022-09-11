using Unity.Netcode;

namespace MainMenu
{
    /// <summary>
    /// Manages network setup in main menu
    /// </summary>
    public class MainMenuManager : NetworkBehaviour
    {
        public void OnClickStartHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void OnClickStartClient()
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}
