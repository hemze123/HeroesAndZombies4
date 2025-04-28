using UnityEngine.Events;

public static class GameEvents
{
    // Oyun Durumu
    public static UnityEvent<GameManager.GameState> OnGameStateChanged = new UnityEvent<GameManager.GameState>();
    
    // Oyuncu
    public static UnityEvent OnPlayerDamaged = new UnityEvent();
    public static UnityEvent<int> OnPlayerDeath = new UnityEvent<int>();
    public static UnityEvent OnPlayerRespawn = new UnityEvent();
    public static UnityEvent<int> OnOutfitChanged = new UnityEvent<int>();
    
    // Silah
    public static UnityEvent<Weapon> OnWeaponChanged = new UnityEvent<Weapon>();
    public static UnityEvent<Weapon, int> OnWeaponUpgraded = new UnityEvent<Weapon, int>();
    
    // Seviye
    public static UnityEvent<int> OnLevelCompleted = new UnityEvent<int>();
    public static UnityEvent<string> OnSceneLoaded = new UnityEvent<string>();
}