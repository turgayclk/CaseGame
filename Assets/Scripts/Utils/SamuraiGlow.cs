using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SamuraiPulseUI : MonoBehaviour
{
    [SerializeField] private Image samuraiImage;      // UI Image
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float scaleAmount = 1.05f;
    [SerializeField] private float interval = 5f;     // 5 saniyede bir

    private Vector3 originalScale;

    private void Start()
    {
        if (samuraiImage == null)
            samuraiImage = GetComponent<Image>();

        originalScale = samuraiImage.rectTransform.localScale;

        // 5 saniyede bir animasyonu baþlat
        InvokeRepeating(nameof(PlayPulse), interval, interval);
    }

    private void PlayPulse()
    {
        Sequence seq = DOTween.Sequence();

        // Scale up
        seq.Append(samuraiImage.rectTransform.DOScale(originalScale * scaleAmount, 0.5f).SetEase(Ease.InOutSine));
        // Shake
        seq.Append(samuraiImage.rectTransform.DOShakePosition(shakeDuration, strength: 5f, vibrato: 10, randomness: 90, fadeOut: true));
        // Scale back
        seq.Append(samuraiImage.rectTransform.DOScale(originalScale, 0.5f).SetEase(Ease.InOutSine));
    }

    private void OnDisable()
    {
        samuraiImage.DOKill();
        samuraiImage.rectTransform.localScale = originalScale;
        CancelInvoke(nameof(PlayPulse));
    }
}
