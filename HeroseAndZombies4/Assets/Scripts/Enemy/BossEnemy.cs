using UnityEngine;

public class BossEnemy : Enemy
{
    [Header("Boss Settings")]
    [SerializeField] private int phase2HealthThreshold = 50;
    [SerializeField] private float enragedSpeedMultiplier = 1.5f;
    [SerializeField] private GameObject[] minionPrefabs;
    [SerializeField] private GameObject enrageEffect;
    [SerializeField] private AudioClip enrageSound;

    private bool isEnraged;
    private EnemyAI enemyAI;

    protected override void Awake()
    {
        base.Awake();
        enemyAI = GetComponent<EnemyAI>();
    }

    public override void TakeDamage(int damageAmount)
    {
        base.TakeDamage(damageAmount);
        
        if(!isEnraged && currentHealth <= phase2HealthThreshold)
            Enrage();
    }

    private void Enrage()
    {
        isEnraged = true;
        enemyAI.SetSpeed(enemyAI.MoveSpeed * enragedSpeedMultiplier);
        SpawnMinions();
        
        if (enrageEffect != null)
            Instantiate(enrageEffect, transform.position, Quaternion.identity);
            
        if (enrageSound != null)
            AudioSource.PlayClipAtPoint(enrageSound, transform.position);
    }

    private void SpawnMinions()
    {
        foreach(GameObject minion in minionPrefabs)
        {
            Vector3 spawnPos = transform.position + Random.insideUnitSphere * 3f;
            spawnPos.y = transform.position.y;
            Instantiate(minion, spawnPos, Quaternion.identity);
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, phase2HealthThreshold/maxHealth * attackRange * 2f);
    }
}