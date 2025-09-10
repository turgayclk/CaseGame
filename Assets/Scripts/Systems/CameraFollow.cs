using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // Takip edilecek player

    [Header("Offset & Settings")]
    public Vector3 offset = new Vector3(0f, 10f, -10f); // Kameranýn player'a göre sabit yüksekliði ve uzaklýðý
    public float followSpeed = 5f; // Smooth hareket hýzý

    private void LateUpdate()
    {
        if (target == null) return;

        // Sadece X ve Z’yi player’dan alýyoruz, Y sabit kalacak
        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            transform.position.y, // Y sabit
            target.position.z + offset.z
        );

        // Smooth hareket
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Rotasyonu sabit býrak
        transform.rotation = Quaternion.Euler(60.5781097f, 89.5797501f, 359.816162f);
    }
}
