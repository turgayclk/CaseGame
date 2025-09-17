using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [SerializeField] private List<Transform> activeEnemies = new List<Transform>();

    public static event Action OnEnemyCountChanged; 

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterEnemy(Transform enemy)
    {
        if (!activeEnemies.Contains(enemy))
        {
            activeEnemies.Add(enemy);
            OnEnemyCountChanged?.Invoke();
        }
    }

    public void UnregisterEnemy(Transform enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
            OnEnemyCountChanged?.Invoke();
        }
    }

    public List<Transform> GetActiveEnemies()
    {
        var currentlyActive = activeEnemies
            .Where(e => e != null && e.gameObject.activeInHierarchy)
            .ToList();

        return currentlyActive;
    }
}
