using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

namespace Misc
{
    /// <summary>
    /// Handles the load and error screens
    /// </summary>
    public class CanvasUtilities : MonoBehaviour
    {
        public static CanvasUtilities Instance;

        [SerializeField] private CanvasGroup loader;
        [SerializeField] private float fadeTime;
        [SerializeField] private TMP_Text loaderText, errorText;

        private TweenerCore<float, float, FloatOptions> _tween;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Toggle(false, instant: true);

            SetCursor(cursorTexture);
        }

        public void Toggle(bool on, string text = null, bool instant = false)
        {
            loaderText.text = text;
            loader.gameObject.SetActive(on);
            _tween?.Kill();
            _tween = loader.DOFade(on ? 1 : 0, instant ? 0 : fadeTime);
        }

        public void ShowError(string error)
        {
            errorText.text = error;
            errorText.DOFade(1, fadeTime).OnComplete(() => { errorText.DOFade(0, fadeTime).SetDelay(1); });
        }

        #region Cursor

        [SerializeField] private Texture2D cursorTexture, clickedCursorTexture;
        private const CursorMode CursorMode = UnityEngine.CursorMode.Auto;
        private readonly Vector2 _hotSpot = Vector2.zero;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) SetCursor(clickedCursorTexture);
            else if (Input.GetMouseButtonUp(0)) SetCursor(cursorTexture);
        }

        private void SetCursor(Texture2D tex) => Cursor.SetCursor(tex, _hotSpot, CursorMode);

        #endregion
    }

    public class Load : IDisposable
    {
        public Load(string text)
        {
            CanvasUtilities.Instance.Toggle(true, text);
        }

        public void Dispose()
        {
            CanvasUtilities.Instance.Toggle(false);
        }
    }
}