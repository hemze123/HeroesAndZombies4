using UnityEngine;

[CreateAssetMenu(fileName = "New Market Item", menuName = "Game/Market Item")]
public class MarketItem : ScriptableObject
{
    public enum ItemType { Weapon, Skin, Consumable }

    [Header("Basic Settings")]
    public string ItemId;
    public ItemType type;
    public string displayName;
    public string description;
    public Sprite icon;
    public GameObject prefab;

    [Header("Pricing")]
    public int basePrice = 100;
    public float priceIncreaseMultiplier = 1.2f;

    [Header("Status")]
    [SerializeField] private bool _isUnlocked;
    [SerializeField] private int _timesPurchased;

    public bool IsUnlocked => _isUnlocked;
    public int TimesPurchased => _timesPurchased;
    public int CurrentPrice => Mathf.RoundToInt(basePrice * Mathf.Pow(priceIncreaseMultiplier, _timesPurchased));

    public void Unlock()
    {
        _isUnlocked = true;
        _timesPurchased++;
    }

    public void LoadData(bool isUnlocked, int timesPurchased)
    {
        _isUnlocked = isUnlocked;
        _timesPurchased = timesPurchased;
    }
}
