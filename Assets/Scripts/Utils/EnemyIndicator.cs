using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EnemyIndicator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Image indicatorIcon; // UI Image (ikon)
    [SerializeField] private Transform player;    // Player referansý

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;

        if (indicatorIcon != null)
            indicatorIcon.enabled = false; // baþta gizli olsun
    }

    private void Update()
    {
        if (EnemyManager.Instance == null || player == null)
        {
            indicatorIcon.enabled = false;
            return;
        }

        var enemies = EnemyManager.Instance.GetActiveEnemies();

        if (enemies == null || enemies.Count == 0)
        {
            indicatorIcon.enabled = false;
            return;
        }

        // En yakýný seç
        Transform closestEnemy = enemies
            .OrderBy(e => Vector3.Distance(player.position, e.position))
            .FirstOrDefault();

        if (closestEnemy == null)
        {
            indicatorIcon.enabled = false;
            return;
        }

        // Enemy ekranda mý kontrol et
        Vector3 viewportPos = mainCam.WorldToViewportPoint(closestEnemy.position);
        bool isVisible = viewportPos.z > 0 && viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1;

        if (isVisible)
        {
            indicatorIcon.enabled = false; // ekran içindeyse gizle
            return;
        }

        // Player ? Enemy yön vektörü
        Vector3 dir = closestEnemy.position - player.position;

        // Yüksekliði görmezden gelmek için sadece X ve Z kullan
        Vector2 dir2D = new Vector2(dir.x, dir.z).normalized;

        // UI ikonunu aktif et
        indicatorIcon.enabled = true;

        // Ýkon açýsýný hesapla (ters)
        float angle = Mathf.Atan2(-dir2D.y, -dir2D.x) * Mathf.Rad2Deg;
        indicatorIcon.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
