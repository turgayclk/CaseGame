using UnityEngine;
using UnityEngine.UI; // Scrollbar için gerekli

public class EnemyController : MonoBehaviour, IDamageable
{
    private EnemyType type;
    private float currentHealth;
    private int currentWaypointIndex;
    private Transform[] pathPoints;
    private Renderer spriteRenderer;

    [Header("UI")]
    [SerializeField] private Slider healthBar; // Canvas içindeki Scrollbar

    public bool IsAlive => currentHealth > 0;

    public void Initialize(EnemyType enemyType, Transform[] path)
    {
        type = enemyType;
        pathPoints = path;
        currentWaypointIndex = 0;
        currentHealth = type.maxHealth;

        Vector3 pos = new Vector3(Random.Range(-5.35f, -3.8f), pathPoints[0].position.y, pathPoints[0].position.z);
        transform.position = pos;

        // Sprite veya renk uygula
        spriteRenderer = GetComponentInChildren<Renderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.material.color = type.tint;
        }

        if (healthBar != null)
        {
            healthBar.value = 1f; // tam dolu
        }
    }

    public void TakeDamage(float amount)
    {
        if (!IsAlive) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, type.maxHealth); // 0 altýna düþmesini engelle

        // visual flash effect
        if (spriteRenderer != null) StartCoroutine(Flash());

        // HealthBar güncelle
        if (healthBar != null)
        {
            healthBar.value = currentHealth / type.maxHealth; // oran
        }

        Debug.Log($"{type.enemyName} took {amount} damage. HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator Flash()
    {
        if (spriteRenderer == null) yield break;
        var orig = spriteRenderer.material.color;
        spriteRenderer.material.color = Color.white;
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.material.color = orig;
    }

    private void Update()
    {
        if (!IsAlive || pathPoints == null || pathPoints.Length == 0) return;

        Transform target = pathPoints[currentWaypointIndex];
        Vector3 dir = target.position - transform.position;
        dir.y = 0;

        transform.position += dir.normalized * type.moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= pathPoints.Length)
            {
                OnReachEnd();
            }
        }
    }

    private void OnReachEnd()
    {
        Debug.Log($"{type.enemyName} reached the end!");

        // Player'a hasar ver
        var player = Object.FindFirstObjectByType<PlayerHealth>();
        if (player != null)
        {
            player.GetComponent<Health>().TakeDamage(type.damage);
        }

        Destroy(gameObject);
    }

    private void Die()
    {
        Debug.Log($"{type.enemyName} died! +{type.rewardGold} gold");
        Destroy(gameObject);
    }
}
