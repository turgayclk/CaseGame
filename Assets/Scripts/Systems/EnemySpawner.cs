using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private Transform[] pathPoints;
    public EnemyType[] enemyTypes; 

    public float spawnInterval = 2f;
    private float timer = 0f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (enemyTypes.Length == 0) return;

        // Rastgele enemy tipi seç
        EnemyType chosenType = enemyTypes[Random.Range(0, enemyTypes.Length)];

        if (chosenType.prefab == null)
        {
            Debug.LogWarning($"{chosenType.enemyName} prefab eksik!");
            return;
        }

        // Prefab’ý spawn et
        GameObject enemy = Instantiate(chosenType.prefab, pathPoints[0].position, chosenType.prefab.transform.rotation);

        // EnemyController initialize
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.Initialize(chosenType, pathPoints);
        }
        else
        {
            Debug.LogWarning("Enemy prefab’ýnda EnemyController bulunamadý!");
        }
    }
}
