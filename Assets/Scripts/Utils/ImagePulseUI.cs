using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine.EventSystems;

// IPointerEnterHandler ve IPointerExitHandler'ý bu scriptten kaldýrýyoruz
public class ImagePulseUI : MonoBehaviour
{
    [SerializeField] private Image imageUI;
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float scaleAmount = 1.05f;

    [SerializeField] private float minInterval = 5f;
    [SerializeField] private float maxInterval = 10f;

    [SerializeField] private GameObject menuUI;
    [SerializeField] private PointerAreaController pointerAreaController; // Pointer alaný için yeni referans

    private Vector3 originalScale;

    private void Start()
    {
        if (imageUI == null)
            imageUI = GetComponent<Image>();

        originalScale = imageUI.rectTransform.localScale;

        //// Menü UI aktifse periyodik animasyonu baþlat
        //if (menuUI != null && menuUI.activeInHierarchy)
        //{
        //    StartCoroutine(PeriodicPulse());
        //}
    }

    private IEnumerator PeriodicPulse()
    {
        while (true)
        {
            // Pointer alaný aktif deðilse animasyonu çalýþtýr
            if (pointerAreaController != null && !pointerAreaController.IsPointerOver)
            {
                PlayPulseAnimation();
            }

            float randomInterval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(randomInterval);
        }
    }

    // Hem periyodik hem de fare imleci için kullanýlacak ortak animasyon metodu
    public void PlayPulseAnimation() // Public yapýyoruz ki baþka scriptten çaðýrýlabilsin
    {
        imageUI.DOKill();

        Sequence seq = DOTween.Sequence();
        seq.Append(imageUI.rectTransform.DOScale(originalScale * scaleAmount, 0.5f).SetEase(Ease.InOutSine));
        seq.Append(imageUI.rectTransform.DOShakePosition(shakeDuration, strength: 5f, vibrato: 10, randomness: 90, fadeOut: true));
        seq.Append(imageUI.rectTransform.DOScale(originalScale, 0.5f).SetEase(Ease.InOutSine));
    }

    public void StopAnimation() // Animasyonu durdurmak için yeni metot
    {
        imageUI.DOKill();
        imageUI.rectTransform.localScale = originalScale;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        if (imageUI != null)
        {
            imageUI.DOKill();
            if (imageUI.rectTransform != null)
            {
                imageUI.rectTransform.localScale = originalScale;
            }
        }
    }
}