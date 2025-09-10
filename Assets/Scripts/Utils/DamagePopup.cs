using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshPro damageText;

    public void Setup(float damage)
    {
        damageText.text = $"-{damage}";
        damageText.alpha = 1;

        // Yukarý doðru hareket ve fade
        transform.DOMoveY(transform.position.y + 2f, 4f).SetEase(Ease.OutCubic);
        damageText.DOFade(0, 4f).OnComplete(() => Destroy(gameObject));
    }
}
