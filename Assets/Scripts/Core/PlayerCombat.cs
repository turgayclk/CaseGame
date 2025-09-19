using DG.Tweening;
using System.Collections; // IEnumerator için bu kütüphaneyi ekle
using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerCombat : MonoBehaviour
{
    [Header("Attack")]
    public float attackRange = 1f;
    public float attackCooldown = 0.8f;
    public float animationDuration = 0.45f; // Animasyon süresi
    public LayerMask enemyLayer;

    private float cooldownTimer = 0f;
    private bool isAttacking = false;
    private Animator animator;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSoundEffect;

    [Header("Camera Shake")]
    [SerializeField] private Transform cameraRigTransform; // CameraRig objesinin Transform bileþeni
    [SerializeField] private float shakeDuration = 0.2f;    // Sarsýntý süresi
    [SerializeField] private float shakeStrength = 0.5f;    // Sarsýntý gücü

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver) return;

        // Eðer þu an cooldown içindeysek bekle
        if (isAttacking)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isAttacking = false;
            }
            return;
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
                // Saldýrý Coroutine'ini baþlat
                StartCoroutine(AttackCoroutine(nearest));
            }
        }
    }

    private IEnumerator AttackCoroutine(Collider target)
    {
        // Cooldown baþlat
        isAttacking = true;
        cooldownTimer = attackCooldown;

        // Saldýrý animasyonunu baþlat
        animator.SetTrigger("AttackTrigger");

        // Animasyon süresi kadar bekle
        yield return new WaitForSeconds(animationDuration);

        // Saldýrý sesi çal
        if (audioSource != null && attackSoundEffect != null)
        {
            audioSource.PlayOneShot(attackSoundEffect);
        }

        // Kamera sarsýntý efektini baþlat
        if (cameraRigTransform != null)
        {
            cameraRigTransform.DOShakePosition(shakeDuration, shakeStrength);
        }

        // Hasar verme ve diðer iþlemleri gerçekleþtir
        float randAttackDmg = Random.Range(7, 25);

        DamagePopupManager.Instance.ShowPopup(randAttackDmg, target.transform.position);

        var dmg = target.GetComponent<IDamageable>();
        if (dmg != null)
        {
            Debug.Log("Enemy Controller Hit!");
            dmg.TakeDamage(randAttackDmg);
        }
    }

    // debug hitbox
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}