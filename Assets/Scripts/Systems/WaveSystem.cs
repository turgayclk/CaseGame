using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveSystem
{
    public Wave firstWave;             // Inspector'dan eklenen ilk wave
    public float enemyCountMultiplier = 1.2f; // Dalga artt�k�a enemy say�s� art���

    public EnemyType normalEnemy;
    public EnemyType speedEnemy;
    public EnemyType tankEnemy;

    // �stenen dalga indexine g�re wave �ret
    public Wave GenerateWave(int waveIndex)
    {
        Wave newWave = new Wave();
        newWave.waveName = $"Wave {waveIndex + 1}";

        List<WaveEnemy> enemies = new List<WaveEnemy>();

        // NormalEnemy say�s� art�yor
        WaveEnemy normal = new WaveEnemy();
        normal.enemyType = normalEnemy;
        normal.count = Mathf.CeilToInt(firstWave.enemies[0].count * Mathf.Pow(enemyCountMultiplier, waveIndex));
        enemies.Add(normal);

        // Wave 2 ve sonras� SpeedEnemy ekle
        if (waveIndex >= 1)
        {
            WaveEnemy speed = new WaveEnemy();
            speed.enemyType = speedEnemy;
            speed.count = waveIndex; // 2. dalgada 1, 3. dalgada 2 vs
            enemies.Add(speed);
        }

        // Wave 3 ve sonras� TankEnemy ekle
        if (waveIndex >= 2)
        {
            WaveEnemy tank = new WaveEnemy();
            tank.enemyType = tankEnemy;
            tank.count = waveIndex - 1;
            enemies.Add(tank);
        }

        newWave.enemies = enemies.ToArray();
        newWave.spawnInterval = firstWave.spawnInterval;

        return newWave;
    }
}
