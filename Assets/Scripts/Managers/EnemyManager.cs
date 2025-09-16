using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [SerializeField] private List<Transform> activeEnemies = new List<Transform>();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterEnemy(Transform enemy)
    {
        if (!activeEnemies.Contains(enemy))
            activeEnemies.Add(enemy);
    }

    public void UnregisterEnemy(Transform enemy)
    {
        if (activeEnemies.Contains(enemy))
            activeEnemies.Remove(enemy);
    }

    public List<Transform> GetActiveEnemies()
    {
        // Sadece aktif olanlarý al
        var currentlyActive = activeEnemies.Where(e => e != null && e.gameObject.activeInHierarchy).ToList();

        // Logla
        string log = currentlyActive.Count > 0
            ? "Aktif Enemies: " + string.Join(", ", currentlyActive.Select(e => e.name))
            : "Aktif Enemy yok";
        Debug.Log(log);

        return currentlyActive;
    }
}
