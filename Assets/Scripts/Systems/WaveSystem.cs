using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveSystem
{
    public Wave firstWave;
    public float enemyCountMultiplier = 1.2f;

    public EnemyType normalEnemy;
    public EnemyType speedEnemy;
    public EnemyType tankEnemy;
    public EnemyType bossEnemy;

    public Wave GenerateWave(int waveIndex)
    {
        Wave newWave = new Wave();
        newWave.waveName = $"Wave {waveIndex + 1}";

        List<WaveEnemy> enemies = new List<WaveEnemy>();

        // Eðer 5’in katý dalga ise boss dalgasý yap
        if ((waveIndex + 1) % 5 == 0)
        {
            WaveEnemy boss = new WaveEnemy();
            boss.enemyType = bossEnemy;

            int bossCount = ((waveIndex + 1) / 5);
            boss.count = bossCount;

            enemies.Add(boss);

            newWave.enemies = enemies.ToArray();
            newWave.spawnInterval = firstWave.spawnInterval;
            return newWave;
        }

        // Normal wave hesaplamasý
        WaveEnemy normal = new WaveEnemy();
        normal.enemyType = normalEnemy;
        normal.count = Mathf.CeilToInt(firstWave.enemies[0].count * Mathf.Pow(enemyCountMultiplier, waveIndex));
        enemies.Add(normal);

        if (waveIndex >= 1) 
        {
            WaveEnemy speed = new WaveEnemy();
            speed.enemyType = speedEnemy;

            int relativeWave = waveIndex - 1; // 2. wave’de 0’dan baþlasýn
            speed.count = Mathf.CeilToInt(1 * Mathf.Pow(enemyCountMultiplier, relativeWave));
            enemies.Add(speed);
        }

        if (waveIndex >= 2)
        {
            WaveEnemy tank = new WaveEnemy();
            tank.enemyType = tankEnemy;

            int relativeWave = waveIndex - 2; // 3. wave’de 0’dan baþlasýn
            tank.count = Mathf.CeilToInt(1 * Mathf.Pow(enemyCountMultiplier, relativeWave));
            enemies.Add(tank);
        }

        newWave.enemies = enemies.ToArray();
        newWave.spawnInterval = firstWave.spawnInterval;

        return newWave;
    }
}
