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
                    float randAttackDmg = Random.Range(7, 25);

                    DamagePopupManager.Instance.ShowPopup(randAttackDmg, nearest.transform.position);

                    animator.SetTrigger("AttackTrigger");
                    var dmg = nearest.GetComponent<IDamageable>();
                    if (dmg != null)
                    {
                        Debug.Log("Enemy Controller Hit!");
                        dmg.TakeDamage(randAttackDmg);
                    }
                    else
                    {
                        Debug.Log("Enemy Hit! : Health Script");

                        // eðer Health component varsa:
                        var h = nearest.GetComponent<Health>();
                        if (h != null) h.TakeDamage(randAttackDmg);
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
