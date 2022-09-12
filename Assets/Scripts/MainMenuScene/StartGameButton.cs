using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenuScene
{
    /// <summary>
    /// Loads the game scene on click
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class StartGameButton : MonoBehaviour
    {
        private void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(LoadGameScene);
        }

        private static void LoadGameScene()
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
