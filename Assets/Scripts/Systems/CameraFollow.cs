using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // Takip edilecek player

    [Header("Offset & Settings")]
    public Vector3 offset = new Vector3(0f, 10f, -10f); // Kameran�n player'a g�re sabit y�ksekli�i ve uzakl���
    public float followSpeed = 5f; // Smooth hareket h�z�

    private void LateUpdate()
    {
        if (target == null) return;

        // Sadece X ve Z�yi player�dan al�yoruz, Y sabit kalacak
        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            transform.position.y, // Y sabit
            target.position.z + offset.z
        );

        // Smooth hareket
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Rotasyonu sabit b�rak
        transform.rotation = Quaternion.Euler(60.5781097f, 89.5797501f, 359.816162f);
    }
}
