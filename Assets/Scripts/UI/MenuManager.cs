using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using DG.Tweening; // DOTween için

public class MenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject settingsUI;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private AudioSource buttonClickSource;

    [Header("Audio Sources")]
    [SerializeField] private List<AudioSource> audioSources;
    [SerializeField] private AudioSource menuMusic;

    private bool isMenuOpen = true;

    private void Start()
    {
        ApplyAudioState();

        playButton.onClick.AddListener(OnPlayClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        exitButton.onClick.AddListener(OnExitClicked);

        // Settings paneli baþta kapalý olsun
        if (settingsUI != null)
            settingsUI.SetActive(false);
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

        ApplyAudioState();
    }

    private void ApplyAudioState()
    {
        if (menuMusic != null)
        {
            menuMusic.enabled = isMenuOpen;
            if (isMenuOpen && !menuMusic.isPlaying)
                menuMusic.Play();
            else if (!isMenuOpen)
                menuMusic.Stop();
        }

        foreach (var source in audioSources)
        {
            if (source == null) continue;

            source.enabled = !isMenuOpen;
            if (!isMenuOpen)
                source.Play();
            else
                source.Stop();
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.HandleMusic();
        }
    }

    private void OnPlayClicked()
    {
        buttonClickSource.Play();

        if (menuUI != null)
            menuUI.SetActive(false);

        isMenuOpen = false;

        if (menuMusic != null)
            menuMusic.Stop();

        foreach (var source in audioSources)
        {
            if (source != null)
            {
                source.enabled = true;
                source.Play();
            }
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.HandleMusic();
        }
    }

    private void OnSettingsClicked()
    {
        buttonClickSource.Play();

        if (settingsUI != null)
        {
            settingsUI.SetActive(true);

            // Fade + Scale animasyonu
            CanvasGroup cg = settingsUI.GetComponent<CanvasGroup>();
            if (cg == null) cg = settingsUI.AddComponent<CanvasGroup>();

            RectTransform rect = settingsUI.GetComponent<RectTransform>();

            cg.alpha = 0f;
            rect.localScale = Vector3.zero;

            Sequence seq = DOTween.Sequence();
            seq.Append(cg.DOFade(1f, 0.5f));
            seq.Join(rect.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
        }
    }

    public void CloseSettings()
    {
        buttonClickSource.Play();

        if (settingsUI != null)
        {
            CanvasGroup cg = settingsUI.GetComponent<CanvasGroup>();
            if (cg == null) cg = settingsUI.AddComponent<CanvasGroup>();

            RectTransform rect = settingsUI.GetComponent<RectTransform>();

            // Animasyonu tersten oynat
            Sequence seq = DOTween.Sequence();
            seq.Append(cg.DOFade(0f, 0.4f));
            seq.Join(rect.DOScale(Vector3.zero, 0.4f).SetEase(Ease.InBack));

            // Animasyon bitince paneli kapat
            seq.OnComplete(() =>
            {
                settingsUI.SetActive(false);
            });
        }
    }

    private void OnExitClicked()
    {
        buttonClickSource.Play();
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
