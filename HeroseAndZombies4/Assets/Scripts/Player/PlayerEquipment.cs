using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public static PlayerEquipment Instance { get; private set; }

    [Header("References")]
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private WeaponManager weaponManager;

    public GameObject CurrentWeapon { get; private set; }
    public GameObject CurrentSkin { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void EquipItem(MarketItem item)
    {
        switch (item.type)
        {
            case MarketItem.ItemType.Weapon:
                CurrentWeapon = item.prefab;
                weaponManager.EquipWeapon(item.prefab.GetComponent<Weapon>());
                break;
                
            case MarketItem.ItemType.Skin:
                CurrentSkin = item.prefab;
                playerManager.ChangeOutfit(GetOutfitIndex(item));
                break;
        }
    }

    private int GetOutfitIndex(MarketItem item)
    {
        // Burada item.prefab'a göre doğru outfit index'ini bul
        // Örnek implementasyon:
        for (int i = 0; i < playerManager.outfits.Length; i++)
        {
            if (playerManager.outfits[i].modelPrefab == item.prefab)
                return i;
        }
        return 0;
    }
}