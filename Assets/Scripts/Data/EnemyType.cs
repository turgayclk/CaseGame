using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Type")]
public class EnemyType : ScriptableObject
{
    [Header("Identity")]
    public string enemyName;
    public Sprite sprite;
    public Color tint = Color.white;
    public string poolTag;

    [Header("Prefab")]
    public GameObject prefab; 

    [Header("Gameplay")]
    public float maxHealth = 10f;
    public float moveSpeed = 2f;
    public float damage = 1f;

    [Header("Optional")]
    public int rewardGold = 1;
}
