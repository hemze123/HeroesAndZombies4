using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game Data/Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("Basic Settings")]
    public string enemyName;
    public GameObject prefab;
    public EnemyType enemyType;
   

    [Header("Combat Stats")]
    public int maxHealth = 100;
    public int damage = 10;
    public float moveSpeed = 3.5f;
    public int coinReward = 5;

    [Header("AI Settings")]
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float patrolSpeed = 2f;

    [Header("Boss Settings")]
    public bool isBoss = false;
    public int phase2HealthThreshold = 50;
    public GameObject[] minionsToSpawn;
}