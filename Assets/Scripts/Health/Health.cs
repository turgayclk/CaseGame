using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Health : MonoBehaviour, IDamageable
{
    public float maxHealth = 10f;
    public float currentHealth { get; private set; }

    public static event Action OnDeath;

    [Header("I-Frames (optional)")]
    public float iFrameDuration = 0.5f;
    private float iFrameTimer;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (iFrameTimer > 0f) iFrameTimer -= Time.deltaTime;
    }

    public bool IsAlive => currentHealth > 0f;

    public void TakeDamage(float amount)
    {
        if (iFrameTimer > 0f) return; // invulnerable

        currentHealth -= amount;

        currentHealth = Mathf.Max(currentHealth, 0f); // Sa�l�k 0'�n alt�na d��mesin

        Debug.Log($"{gameObject.name} took {amount} damage. HP: {currentHealth}");

        // (iste�e ba�l�) visual flash:
        if (sr != null) StartCoroutine(Flash());

        if (iFrameDuration > 0f) iFrameTimer = iFrameDuration;

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
        }
    }

    public void Revive()
    {
        currentHealth = maxHealth;
        Debug.Log("Revived with full HP!");

        // Ge�ici olarak damage kapal�
        StartCoroutine(ReviveRoutine());
    }

    private System.Collections.IEnumerator ReviveRoutine()
    {
        float duration = 2f;
        iFrameTimer = duration;

        if (sr != null)
        {
            // �nce varsa eski tween'i iptal et
            sr.DOKill();

            // Yan�p s�nme efekti: alpha 1 - 0.3 gidip gelecek
            sr.DOFade(0.3f, 0.3f)
              .SetLoops(-1, LoopType.Yoyo)
              .SetEase(Ease.InOutSine);
        }

        yield return new WaitForSeconds(duration);

        if (sr != null)
        {
            sr.DOKill(); // tween'i bitir
            sr.DOFade(1f, 0.1f); // alpha'y� tamamen eski haline getir
        }
    }

    private System.Collections.IEnumerator Flash()
    {
        if (sr == null) yield break;

        Color original = sr.color;
        Color hitColor = Color.red;
        float duration = 0.3f; // fade s�resi
        float timer = 0f;

        // Hemen k�rm�z�ya ge�
        sr.color = hitColor;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            sr.color = Color.Lerp(hitColor, original, timer / duration);
            yield return null;
        }

        sr.color = original; // kesin olarak orijinal renk
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    private void Die()
    {
        OnDeath?.Invoke();
        
    }
}
