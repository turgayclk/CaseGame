using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerCombat : MonoBehaviour
{
    [Header("Attack")]
    public float attackRange = 1f;
    public float attackCooldown = 0.8f; // saldýrýlar arasý bekleme
    public LayerMask enemyLayer;

    private float cooldownTimer = 0f;
    private bool isAttacking = false;
    private Animator animator;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSoundEffect;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver) return;

        // Eðer þu an cooldown içindeysek zaman say
        if (isAttacking)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isAttacking = false; // tekrar saldýrabilir
            }
            return; // cooldown bitene kadar çýk
        }

        // Yeni saldýrý baþlatabilir mi?
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        if (hits.Length > 0)
        {
            Collider nearest = null;
            float minDist = float.MaxValue;

            foreach (var c in hits)
            {
                float d = Vector3.SqrMagnitude(c.transform.position - transform.position);
                if (d < minDist)
                {
                    minDist = d;
                    nearest = c;
                }
            }

            if (nearest != null)
            {
                animator.SetTrigger("AttackTrigger");

                // Saldýrý sesi çal
                if (audioSource != null && attackSoundEffect != null)
                {
                    audioSource.PlayOneShot(attackSoundEffect);
                }

                float randAttackDmg = Random.Range(7, 25);

                DamagePopupManager.Instance.ShowPopup(randAttackDmg, nearest.transform.position);

                var dmg = nearest.GetComponent<IDamageable>();
                if (dmg != null)
                {
                    Debug.Log("Enemy Controller Hit!");
                    dmg.TakeDamage(randAttackDmg);
                }

                // Cooldown baþlat
                isAttacking = true;
                cooldownTimer = attackCooldown;
            }
        }
    }

    // debug hitbox
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
