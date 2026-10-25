using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
public class FadeSceneManager : MonoBehaviour
{
    public static FadeSceneManager Instance { get; private set; }

    [Header("Fade Settings")]
    [Tooltip("Duration of each fade in seconds (to black / back from black).")]
    [SerializeField] private float fadeDuration = 0.5f;

    [Tooltip("Extra time to hold the black screen after the scene has loaded (in seconds).")]
    [SerializeField] private float holdOnBlack = 0f;

    [Tooltip("Color of the fade overlay (default: black).")]
    [SerializeField] private Color fadeColor = Color.black;

    public bool IsFading { get; private set; }
    public event Action OnBeforeSceneChange;
    public event Action OnAfterSceneChange;

    private CanvasGroup fadeGroup;

    void Start()
    {
        AudioManager.Instance.PlayMusic("Game");
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        CreateFadeOverlay();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void LoadScene(string sceneName)
    {
        if (IsFading)
        {
            Debug.LogWarning("[FadeSceneManager] Transition already in progress — request ignored.");
            return;
        }
        StartCoroutine(FadeAndLoad(() => SceneManager.LoadSceneAsync(sceneName)));
    }

    public void LoadScene(int sceneIndex)
    {
        if (IsFading)
        {
            Debug.LogWarning("[FadeSceneManager] Transition already in progress — request ignored.");
            return;
        }
        StartCoroutine(FadeAndLoad(() => SceneManager.LoadSceneAsync(sceneIndex)));
    }

    public void RestartScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        if (IsFading)
        {
            Debug.LogWarning("[FadeSceneManager] Transition already in progress — request ignored.");
            return;
        }
        StartCoroutine(FadeAndQuit());
    }

    private IEnumerator FadeAndLoad(Func<AsyncOperation> loadOperation)
    {
        IsFading = true;

        yield return Fade(0f, 1f);

        OnBeforeSceneChange?.Invoke();

        AsyncOperation op = loadOperation();
        if (op == null)
        {
            Debug.LogError("[FadeSceneManager] Scene failed to load. " +
                           "Check that the scene name/index exists in Build Settings / Build Profiles.");
        }
        else
        {
            while (!op.isDone)
                yield return null;
        }

        if (holdOnBlack > 0f)
            yield return new WaitForSecondsRealtime(holdOnBlack);

        yield return Fade(1f, 0f);

        IsFading = false;
        OnAfterSceneChange?.Invoke();
    }

    private IEnumerator FadeAndQuit()
    {
        IsFading = true;
        yield return Fade(0f, 1f);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    private IEnumerator Fade(float from, float to)
    {
        fadeGroup.blocksRaycasts = true;

        if (fadeDuration <= 0f)
        {
            fadeGroup.alpha = to;
        }
        else
        {
            float t = 0f;
            while (t < 1f)
            {
                t = Mathf.Clamp01(t + Time.unscaledDeltaTime / fadeDuration);
                fadeGroup.alpha = Mathf.Lerp(from, to, t);
                yield return null;
            }
        }

        fadeGroup.alpha = to;
        fadeGroup.blocksRaycasts = to > 0f;
    }

    private void CreateFadeOverlay()
    {
        GameObject canvasGO = new GameObject("FadeCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 32767; // topmost
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        GameObject imageGO = new GameObject("FadeImage");
        RectTransform rect = imageGO.AddComponent<RectTransform>();
        rect.SetParent(canvasGO.transform, false);
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image = imageGO.AddComponent<Image>();
        image.color = fadeColor;
        image.raycastTarget = false;

        fadeGroup = imageGO.AddComponent<CanvasGroup>();
        fadeGroup.alpha = 0f;
        fadeGroup.blocksRaycasts = false;
        fadeGroup.interactable = false;

        canvasGO.transform.SetParent(transform, false);
    }
}
