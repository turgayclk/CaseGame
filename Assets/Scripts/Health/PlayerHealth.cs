using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Component & Death UI")]
    [SerializeField] private Health health;
    [SerializeField] private GameObject deathUI;

    [Header("Health Bar UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;

    [Header("Health Text UI")]
    [SerializeField] private TextMeshProUGUI healthText;

    private float previousHealth;

    private void Awake()
    {
        if (health == null)
            health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        Health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        Health.OnDeath -= HandleDeath;
    }

    public void Revive()
    {
        deathUI.SetActive(false);
        health.Revive();
        UpdateHealthTextInstant();
    }

    private void Start()
    {
        if (healthSlider != null)
            healthSlider.value = 1f;

        previousHealth = health.maxHealth;
        UpdateHealthTextInstant();
    }

    private void Update()
    {
        if (healthSlider != null && health != null)
        {
            float targetValue = health.currentHealth / health.maxHealth;

            if (!Mathf.Approximately(targetValue, healthSlider.value))
            {
                healthSlider.DOValue(targetValue, 0.8f).SetEase(Ease.OutCubic);
            }

            // Text için animasyonlu geçiþ
            AnimateHealthText((int)previousHealth, (int)health.currentHealth);

            previousHealth = health.currentHealth;
        }
    }

    private void AnimateHealthText(int fromValue, int toValue)
    {
        if (healthText == null) return;

        // Ayný deðerse animasyon yapmaya gerek yok
        if (fromValue == toValue) return;

        // Önceki animasyonu iptal et
        DOTween.Kill(healthText);

        // Sayma animasyonu
        DOVirtual.Int(fromValue, toValue, 0.8f, value =>
        {
            healthText.text = value.ToString();
        }).SetEase(Ease.OutCubic).SetTarget(healthText);
    }

    private void UpdateHealthTextInstant()
    {
        if (healthText != null && health != null)
        {
            healthText.text = Mathf.CeilToInt(health.currentHealth).ToString();
        }
    }

    private void HandleDeath()
    {
        Debug.Log("Player died!");

        Time.timeScale = 0f;

        if (healthSlider != null)
        {
            healthSlider.DOKill();
            healthSlider.value = 0f;
            if (fillImage != null)
                fillImage.color = Color.red;
        }

        if (deathUI != null)
            deathUI.SetActive(true);

        if (healthText != null)
            healthText.text = "0";

        GameManager.Instance.OnSceneLoaded();
    }
}
