using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // Takip edilecek player

    [Header("Offset & Settings")]
    public Vector3 offset = new Vector3(0f, 10f, -10f);
    public float followSpeed = 5f;

    private Quaternion fixedRotation;

    private void Start()
    {
        // Kameranýn sabit açýsýný kaydediyoruz
        fixedRotation = Quaternion.Euler(60.5781097f, 89.5797501f, 359.816162f);
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            transform.position.y,
            target.position.z + offset.z
        );

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        transform.rotation = fixedRotation;
    }
}
