using UnityEngine;
using UnityEngine.UI; // Button için gerekli
using UnityEngine.SceneManagement; // Çýkýþ için gerekli (build sýrasýnda)

public class MenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject menuUI;       // Ana menü paneli
    [SerializeField] private GameObject settingsUI;   // Settings paneli

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    [Header("Button Sprites")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite hoverSprite;
    [SerializeField] private Sprite pressedSprite;

    private bool isMenuOpen = false;

    private void Start()
    {
        // Butonlarýn sprite durumlarýný ayarla
        SetupButtonSprites(playButton);
        SetupButtonSprites(settingsButton);
        SetupButtonSprites(exitButton);

        // OnClick eventlerini baðla
        playButton.onClick.AddListener(OnPlayClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        exitButton.onClick.AddListener(OnExitClicked);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        if (menuUI != null)
            menuUI.SetActive(isMenuOpen);
    }

    private void SetupButtonSprites(Button button)
    {
        if (button == null) return;

        SpriteState state = new SpriteState
        {
            highlightedSprite = hoverSprite,
            pressedSprite = pressedSprite
        };

        button.image.sprite = normalSprite;
        button.spriteState = state;
    }

    // --- Buton Fonksiyonlarý ---
    private void OnPlayClicked()
    {
        if (menuUI != null)
            menuUI.SetActive(false); // Menü kapanýr
        Debug.Log("Play clicked -> Game starts");
    }

    private void OnSettingsClicked()
    {
        if (settingsUI != null)
            settingsUI.SetActive(true); // Settings paneli açýlýr
        Debug.Log("Settings clicked -> Open settings");
    }

    private void OnExitClicked()
    {
        Debug.Log("Exit clicked -> Quit game");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Editor'de çalýþýrken oyunu durdurur
#endif
    }
}
