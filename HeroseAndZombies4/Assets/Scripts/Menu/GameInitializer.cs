using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private GameManager gameManagerPrefab;
    [SerializeField] private UIManager uiManagerPrefab;
    [SerializeField] private AudioManager audioManagerPrefab;

    [Header("Debug")]
    [SerializeField] private bool enableDebugCheats = true;

    void Awake()
    {
        InitializeSingletonManagers();
        LinkSystemDependencies();
        
        if (enableDebugCheats && Debug.isDebugBuild)
            gameObject.AddComponent<DebugCheats>();
    }

    void InitializeSingletonManagers()
    {
        if (GameManager.Instance == null)
            Instantiate(gameManagerPrefab);
        
        if (UIManager.Instance == null)
            Instantiate(uiManagerPrefab);
            
        if (AudioManager.Instance == null)
            Instantiate(audioManagerPrefab);
    }

    void LinkSystemDependencies()
    {
        // Player Systems
        Collector.Instance.OnDeath.AddListener(HandlePlayerDeath);
        
        // Weapon Systems
        WeaponManager.Instance.OnWeaponChanged.AddListener(UpdateWeaponUI);
            
        // Economy Systems
        EconomyManager.Instance.OnCoinsChanged.AddListener(UpdateMarketUI);
    }

    void HandlePlayerDeath()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
    }

    void UpdateWeaponUI(Weapon weapon)
    {
        UIManager.Instance.UpdateWeaponUI(weapon);
    }

    void UpdateMarketUI(int coins)
    {
        MarketManager.Instance.UpdateUI();
    }

    void OnDestroy()
    {
        // Event bağlantılarını temizle
        if (Collector.Instance != null)
            Collector.Instance.OnDeath.RemoveListener(HandlePlayerDeath);
            
        if (WeaponManager.Instance != null)
            WeaponManager.Instance.OnWeaponChanged.RemoveListener(UpdateWeaponUI);
            
        if (EconomyManager.Instance != null)
            EconomyManager.Instance.OnCoinsChanged.RemoveListener(UpdateMarketUI);
    }
}