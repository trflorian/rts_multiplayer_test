using Services;
using UnityEngine;

namespace Misc
{
    /// <summary>
    ///     This will run once before any other scene script
    /// </summary>
    public static class Bootstrapper {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize() {
            MatchmakingService.ResetStatics();
            Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("CanvasUtilities")));
        }
    }
}