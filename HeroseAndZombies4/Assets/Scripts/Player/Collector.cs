using UnityEngine;
using UnityEngine.Events;

public class Collector : MonoBehaviour
{
    public static Collector Instance { get; private set; }

    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float healthRegen = 1f;

    [Header("Economy")] 
    [SerializeField] private int startingCoins = 100;

    public UnityEvent OnDeath;
    
    public UnityEvent<float> OnHealthChanged;
    public UnityEvent<int> OnCoinsChanged;
    //

    private bool isInvincible;
    public bool IsInvincible => isInvincible;
    //

    private float currentHealth;
    private int currentCoins;

    void Awake()
    {
        if(Instance != null) Destroy(gameObject);
        else Instance = this;
        
        LoadData();
    }

    void Update() => RegenerateHealth();


    public void SetInvincible(bool state)
{
    isInvincible = state;
    Debug.Log($"Invincibility: {state}");
}

    void RegenerateHealth()
    {
        if(currentHealth < maxHealth)
        {
            currentHealth += healthRegen * Time.deltaTime;
            OnHealthChanged?.Invoke(currentHealth);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        OnHealthChanged?.Invoke(currentHealth);
        
        if(currentHealth <= 0) OnDeath?.Invoke();
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        SaveManager.Instance.SaveInt("player_coins", currentCoins);
        OnCoinsChanged?.Invoke(currentCoins);
        
    }

     public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
        Debug.Log($"Healed: +{amount} HP");
    }
    public void ResetHealth()
    {
    // Sağlığı başlangıç değerine al
    }

    void LoadData()
    {
        currentHealth = maxHealth;
        currentCoins = SaveManager.Instance.LoadInt("player_coins", startingCoins);
    }
    void OnDisable()
{
    Debug.LogWarning("Collector Disable oldu! Time: " + Time.time);
    Debug.Break(); // Oyun anında durur
}
}