using UnityEngine;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class Health : MonoBehaviour, IDamageable
{
    public float maxHealth = 10f;
    public float currentHealth { get; private set; }

    public static event Action OnDeath;

    [Header("I-Frames (optional)")]
    public float iFrameDuration = 0f;
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

    private System.Collections.IEnumerator Flash()
    {
        if (sr == null) yield break;
        var orig = sr.color;
        sr.color = Color.white;
        yield return new WaitForSeconds(0.08f);
        sr.color = orig;
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
