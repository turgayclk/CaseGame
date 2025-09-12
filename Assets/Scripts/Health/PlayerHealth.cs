using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // DOTween eklendi

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Component & Death UI")]
    [SerializeField] private Health health;
    [SerializeField] private GameObject deathUI;

    [Header("Health Bar UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage; // Slider -> Fill alan�ndaki Image

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
    }

    private void Start()
    {
        if (healthSlider != null)
            healthSlider.value = 1f; // ba�lang�� full

        if (fillImage != null)
            fillImage.color = Color.green;

        previousHealth = health.maxHealth;
    }

    private void Update()
    {
        if (healthSlider != null && health != null)
        {
            float targetValue = health.currentHealth / health.maxHealth;

            // sadece can de�i�tiyse animasyon ba�lat
            if (!Mathf.Approximately(targetValue, healthSlider.value))
            {
                healthSlider.DOValue(targetValue, 0.8f).SetEase(Ease.OutCubic);
            }

            // renk de�i�imi
            if (fillImage != null)
            {
                if (targetValue <= 0.25f)
                    fillImage.color = Color.red;
                else if (targetValue <= 0.5f)
                    fillImage.color = Color.yellow;
                else
                    fillImage.color = Color.green;
            }

            previousHealth = health.currentHealth;
        }
    }

    private void HandleDeath()
    {
        Debug.Log("Player died!");

        Time.timeScale = 0f; // oyunu durdur

        // �l�m an�nda slider'� direkt s�f�rla
        if (healthSlider != null)
        {
            healthSlider.DOKill();         // varsa animasyonu iptal et
            healthSlider.value = 0f;       // direkt 0 yap
            if (fillImage != null)
                fillImage.color = Color.red; // k�rm�z�da kals�n
        }

        if (deathUI != null)
            deathUI.SetActive(true);

        GameManager.Instance.OnSceneLoaded();
    }
}
