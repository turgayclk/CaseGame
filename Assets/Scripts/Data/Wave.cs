[System.Serializable]
public class Wave
{
    public string waveName;
    public WaveEnemy[] enemies;  // Bu dalgadaki d��manlar
    public float spawnInterval = 1f; // bu dalgadaki spawn aral���
}

[System.Serializable]
public class WaveEnemy
{
    public EnemyType enemyType;
    public int count; // ka� tane spawn edilecek
}
