using UnityEngine;

public class DebugCheats : MonoBehaviour
{
    void Update()
    {
        HandleDebugInputs();
    }

    void HandleDebugInputs()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            AddCoins(1000);
        
        if (Input.GetKeyDown(KeyCode.F2))
            ToggleGodMode();
        
        if (Input.GetKeyDown(KeyCode.F3))
            SkipLevel();
        
        if (Input.GetKeyDown(KeyCode.F4))
            UnlockAllWeapons();
    }

    void AddCoins(int amount)
    {
        EconomyManager.Instance.AddCoins(amount);
        Debug.Log($"Debug: Added {amount} coins");
    }

    void ToggleGodMode()
    {
        Collector.Instance.SetInvincible(!Collector.Instance.IsInvincible);
        Debug.Log($"Debug: God Mode {(Collector.Instance.IsInvincible ? "ON" : "OFF")}");
    }

    void SkipLevel()
    {
        LevelManager.Instance.CompleteCurrentLevel();
        Debug.Log("Debug: Level skipped");
    }

   void UnlockAllWeapons()
{
    foreach (WeaponData weapon in Resources.LoadAll<WeaponData>("Weapons"))
    {
        // weapon.weaponName veya weapon.id gibi bir alan kullan
        MarketManager.Instance.UnlockItem(weapon.name); // Varsayılan Unity ismi
        // VEYA: MarketManager.Instance.UnlockItem(weapon.weaponId); // Eğer özel bir ID tanımlıysa
    }
    Debug.Log("Debug: Tüm silahlar açıldı!");
}
}