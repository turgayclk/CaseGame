using UnityEngine;
using UnityEngine.UI; // Scrollbar için gerekli
using System.Collections;

public class EnemyController : MonoBehaviour, IDamageable
{
    private EnemyType type;
    private float currentHealth;
    private int currentWaypointIndex;
    private Transform[] pathPoints;
    private Renderer spriteRenderer;
    [SerializeField] private ParticleSystem bloodEffect;

    [Header("UI")]
    [SerializeField] private Slider healthBar; // Canvas içindeki Scrollbar

    public bool IsAlive => currentHealth > 0;

    private Animator animator;

    private bool isStunned = false;   // hareketi durdurmak için
    private Coroutine stunCoroutine;  // tekrar damage alýrsa eski coroutine’i iptal etmek için

    public void Initialize(EnemyType enemyType, Transform[] path)
    {
        type = enemyType;
        pathPoints = path;
        currentWaypointIndex = 0;
        currentHealth = type.maxHealth;

        animator = GetComponent<Animator>();

        Vector3 pos = new Vector3(Random.Range(-5.35f, -3.8f), pathPoints[0].position.y, pathPoints[0].position.z);
        transform.position = pos;

        if (healthBar != null)
        {
            healthBar.value = 1f; // tam dolu
        }

        if (enemyType.isBoss)
        {
            transform.localScale = Vector3.one * 3; // büyük gözüksün
            Vector3 scale = transform.localScale;
            scale.x = scale.x * -1; // boss ters dönük baþlasýn
            transform.localScale = scale;
        }
    }

    private void Start()
    {
        healthBar.gameObject.SetActive(false);
    }

    public void TakeDamage(float amount)
    {
        if (!IsAlive) return;

        healthBar.gameObject.SetActive(true);

        animator.SetTrigger("HurtTrigger");

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, type.maxHealth); // 0 altýna düþmesini engelle

        // visual flash effect
        if (spriteRenderer != null) StartCoroutine(Flash());

        if (bloodEffect != null)
        {
            bloodEffect.Play();
        }

        // HealthBar güncelle
        if (healthBar != null)
        {
            healthBar.value = currentHealth / type.maxHealth; // oran
        }

        Debug.Log($"{type.enemyName} took {amount} damage. HP: {currentHealth}");

        // --- Burada stun baþlatýyoruz ---
        if (stunCoroutine != null) StopCoroutine(stunCoroutine); // eðer eski stun varsa iptal et
        stunCoroutine = StartCoroutine(Stun(0.5f));

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator Stun(float duration)
    {
        isStunned = true;
        yield return new WaitForSeconds(duration);

        // Eðer stun süresince tekrar damage almadýysa - yürümeye devam
        isStunned = false;
    }

    private IEnumerator Flash()
    {
        if (spriteRenderer == null) yield break;

        Color original = spriteRenderer.material.color;
        Color hitColor = Color.red;
        float duration = 0.3f;
        float timer = 0f;

        spriteRenderer.material.color = hitColor;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            spriteRenderer.material.color = Color.Lerp(hitColor, original, timer / duration);
            yield return null;
        }

        spriteRenderer.material.color = original;
    }

    private void Update()
    {
        if (!IsAlive || pathPoints == null || pathPoints.Length == 0) return;
        if (isStunned) return; // hasar aldýysa 0.5 saniye boyunca hareket yok

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

        StartCoroutine(WaitAttackAnim());
    }

    private void Die()
    {
        Debug.Log($"{type.enemyName} died! +{type.rewardGold} gold");

        StartCoroutine(WaitDieAnim());
    }

    IEnumerator WaitAttackAnim()
    {
        animator.SetTrigger("AttackTrigger");
        yield return new WaitForSeconds(0.75f);

        var player = Object.FindFirstObjectByType<Health>(); // direkt Health scriptini bul
        if (player != null)
        {
            player.TakeDamage(type.damage); // Health scriptindeki I-frame zaten devreye girecek
        }

        gameObject.SetActive(false);
    }

    IEnumerator WaitDieAnim()
    {
        healthBar.gameObject.SetActive(false);
        animator.SetTrigger("DieTrigger");

        yield return new WaitForSeconds(1f);
        
        gameObject.SetActive(false);
    }
}
