using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class EnvironmentWindAnimator : MonoBehaviour
{
    [Header("Objects to Animate")]
    [SerializeField] private List<GameObject> environmentObjects = new List<GameObject>();

    [Header("Wind Settings")]
    [SerializeField] private float rotationAmount = 5f; // sað-sol açý
    [SerializeField] private float duration = 2f; // bir sallanma süresi
    [SerializeField] private float delayBetween = 0.2f; // her obje arasýnda ufak gecikme (doðal his için)

    private void Start()
    {
        AnimateObjects();
    }

    private void AnimateObjects()
    {
        for (int i = 0; i < environmentObjects.Count; i++)
        {
            if (environmentObjects[i] == null) continue;

            Transform obj = environmentObjects[i].transform;

            // baþlangýç rotasyonu kaydet
            Vector3 baseRotation = obj.localEulerAngles;

            // rüzgar efekti: Z ekseninde hafif sað-sol döndür
            obj.DOLocalRotate(
                new Vector3(baseRotation.x, baseRotation.y, baseRotation.z + rotationAmount),
                duration
            )
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo) // sonsuz loop
            .SetDelay(i * delayBetween); // aralýklý baþlasýn (daha doðal)
        }
    }
}
