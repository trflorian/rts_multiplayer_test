using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenuScene
{
    /// <summary>
    /// Loads the game scene on click
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class QuitGameButton : MonoBehaviour
    {
        private void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(QuitGame);
        }

        private static void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
