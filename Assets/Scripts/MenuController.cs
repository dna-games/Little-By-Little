using UnityEngine;
using UnityEngine.UIElements;
public class MenuController : MonoBehaviour
{
    [SerializeField] Sprite muteSprite;
    [SerializeField] Sprite unmuteSprite;
    bool isMuted = false;
    UIDocument uiDocument;
    Button startButton;
    Button exitButton;
    Button settingsButton;
    Button muteButton;
    VisualElement buttonsWraper;

    void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        buttonsWraper = uiDocument.rootVisualElement.Q<VisualElement>("Buttons");
        startButton = uiDocument.rootVisualElement.Q<Button>("StartButton");
        exitButton = uiDocument.rootVisualElement.Q<Button>("ExitButton");
        settingsButton = uiDocument.rootVisualElement.Q<Button>("SettingsButton");
        muteButton = uiDocument.rootVisualElement.Q<Button>("MuteButton");
        startButton.clicked += StartGame;
        exitButton.clicked += ExitGame;
        muteButton.clicked += ToggleMute;
    }

    void StartGame()
    {
        FadeSceneManager.Instance.LoadScene(1);
    }
    void ExitGame()
    {
        FadeSceneManager.Instance.QuitGame();
    }

    void ToggleMute()
    {
        AudioListener.pause = !AudioListener.pause;
        isMuted = AudioListener.pause;
        var bg = muteButton.style.backgroundImage;
        bg.value = Background.FromSprite(isMuted ? muteSprite : unmuteSprite);
        muteButton.style.backgroundImage = bg;
    }
}
