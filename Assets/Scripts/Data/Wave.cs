[System.Serializable]
public class Wave
{
    public string waveName;
    public WaveEnemy[] enemies;  // Bu dalgadaki düþmanlar
    public float spawnInterval = 1f; // bu dalgadaki spawn aralýðý
}

[System.Serializable]
public class WaveEnemy
{
    public EnemyType enemyType;
    public int count; // kaç tane spawn edilecek
}
