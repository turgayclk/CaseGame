using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerCombat : MonoBehaviour
{
    [Header("Attack")]
    public float attackRange = 1.2f;
    public float attackCooldown = 0.8f;
    public LayerMask enemyLayer;

    private float cooldownTimer = 0f;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver) return;

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            // en yakýn düþmaný bul
            Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
            if (hits.Length > 0)
            {
                // en yakýn olan
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

                    float randAttackDmg = Random.Range(7, 25);

                    DamagePopupManager.Instance.ShowPopup(randAttackDmg, nearest.transform.position);

                    var dmg = nearest.GetComponent<IDamageable>();
                    if (dmg != null)
                    {
                        Debug.Log("Enemy Controller Hit!");
                        dmg.TakeDamage(randAttackDmg);
                    }

                    // reset cooldown
                    cooldownTimer = attackCooldown;
                    // (opsiyonel) animasyon / vfx tetikle
                }
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
