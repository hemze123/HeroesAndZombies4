using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected int coinReward = 5;
    [SerializeField] protected float attackRange = 2f;
    [SerializeField] protected float attackCooldown = 1f;
    
    [Header("Components")]
    [SerializeField] protected EnemyHealthBar healthBar;
    [SerializeField] protected GameObject deathEffect;
    [SerializeField] protected AudioClip hitSound;

    protected int currentHealth;
    protected Transform player;
    protected float lastAttackTime;
    protected NavMeshAgent agent;
    
    public UnityEvent OnDeath;
    public UnityEvent OnTakeDamage;
    public int CurrentHealth => currentHealth; // Dış erişim için property

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
        
        InitializeHealthBar();
    }

    private void InitializeHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.Initialize(this, maxHealth);
        }
    }

  public virtual void TakeDamage(int damageAmount)
{
    currentHealth -= damageAmount;
    OnTakeDamage?.Invoke();
    
    if (healthBar != null)
        healthBar.HandleDamageTaken(); // Değiştirilen kısım
    
    if (hitSound != null)
        AudioSource.PlayClipAtPoint(hitSound, transform.position);

    if (currentHealth <= 0) 
        Die();
}

    protected virtual void Die()
    {
        Collector.Instance.AddCoins(coinReward);
        
        if (deathEffect != null) 
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    public virtual void AttackPlayer()
    {
        if (Time.time - lastAttackTime < attackCooldown) 
            return;
        
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            player.GetComponent<IDamageable>()?.TakeDamage(damage);
            lastAttackTime = Time.time;
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return player.position;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}