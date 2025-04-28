using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MarketManager : MonoBehaviour
{
    public static MarketManager Instance { get; private set; }

    [System.Serializable]
    public class MarketItemData
    {
        public string itemId;
        public bool isUnlocked;
        public int timesPurchased;
    }

    [Header("Item Settings")]
    [SerializeField] private List<MarketItem> allItems = new List<MarketItem>();

    [Header("Events")]
    public UnityEvent<MarketItem> OnItemPurchased = new UnityEvent<MarketItem>();
    public UnityEvent OnInventoryUpdated = new UnityEvent();

    private List<MarketItem> unlockedItems = new List<MarketItem>();
    private const string SAVE_KEY = "MarketData";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadUnlockedItems();
    }
    public void UpdateUI()
     {
    // UI güncelleme işlemlerini burada yapabilirsin
    OnInventoryUpdated?.Invoke();
    }


    public bool PurchaseItem(MarketItem item)
    {
        if (item == null || item.IsUnlocked) return false;

        int price = item.CurrentPrice;
        if (EconomyManager.Instance.SpendCoins(price))
        {
            item.Unlock();
            unlockedItems.Add(item);
            SaveItems();

            OnItemPurchased?.Invoke(item);
            OnInventoryUpdated?.Invoke();
            return true;
        }
        return false;
    }

    public void UnlockItem(string itemId)
    {
        MarketItem item = allItems.Find(i => i.ItemId == itemId);
        if (item != null && !item.IsUnlocked)
        {
            item.Unlock();
            unlockedItems.Add(item);
            SaveItems();
            OnInventoryUpdated?.Invoke();
        }
    }

    public List<MarketItem> GetAvailableItems() => new List<MarketItem>(allItems);
    public List<MarketItem> GetUnlockedItems() => new List<MarketItem>(unlockedItems);

    private void SaveItems()
    {
        List<MarketItemData> saveData = new List<MarketItemData>();
        foreach (var item in unlockedItems)
        {
            saveData.Add(new MarketItemData
            {
                itemId = item.ItemId,
                isUnlocked = item.IsUnlocked,
                timesPurchased = item.TimesPurchased
            });
        }
        string json = JsonUtility.ToJson(new SerializationWrapper<MarketItemData>(saveData));
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    private void LoadUnlockedItems()
    {
        unlockedItems.Clear();
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            var wrapper = JsonUtility.FromJson<SerializationWrapper<MarketItemData>>(json);

            if (wrapper != null && wrapper.items != null)
            {
                foreach (var data in wrapper.items)
                {
                    MarketItem item = allItems.Find(i => i.ItemId == data.itemId);
                    if (item != null)
                    {
                        item.LoadData(data.isUnlocked, data.timesPurchased);
                        unlockedItems.Add(item);
                    }
                }
            }
        }
    }

    [System.Serializable]
    private class SerializationWrapper<T>
    {
        public List<T> items;
        public SerializationWrapper(List<T> list) => items = list;
    }
}
