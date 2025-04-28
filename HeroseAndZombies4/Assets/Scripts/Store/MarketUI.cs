using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MarketUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private GameObject itemButtonPrefab;
    [SerializeField] private TMP_Text playerCoinsText;
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Text categoryTitle;

    [Header("Tabs")]
    [SerializeField] private Button weaponsTab;
    [SerializeField] private Button skinsTab;
    [SerializeField] private Button consumablesTab;

    private MarketItem.ItemType currentCategory = MarketItem.ItemType.Weapon;

    void Awake()
    {
        // Tab butonlarını ayarla
        weaponsTab.onClick.AddListener(() => ShowCategory(MarketItem.ItemType.Weapon));
        skinsTab.onClick.AddListener(() => ShowCategory(MarketItem.ItemType.Skin));
        consumablesTab.onClick.AddListener(() => ShowCategory(MarketItem.ItemType.Consumable));
        
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        EconomyManager.Instance.OnCoinsChanged.AddListener(UpdateCoinsDisplay);
        
    }

    void OnEnable()
    {
        UpdateCoinsDisplay(EconomyManager.Instance.GetCurrentCoins());
        ShowCategory(currentCategory);
    }

    void ShowCategory(MarketItem.ItemType category)
    {
        currentCategory = category;
        ClearItemsContainer();
        
        List<MarketItem> items = MarketManager.Instance.GetAvailableItems();
        int displayedItems = 0;

        foreach (MarketItem item in items)
        {
            if (item.type == category)
            {
                CreateItemButton(item);
                displayedItems++;
            }
        }

        categoryTitle.text = $"{category.ToString()} ({displayedItems})";
    }

    void CreateItemButton(MarketItem item)
    {
        GameObject buttonObj = Instantiate(itemButtonPrefab, itemsContainer);
        MarketItemUI itemUI = buttonObj.GetComponent<MarketItemUI>();
        itemUI.Initialize(item);
    }

    void ClearItemsContainer()
    {
        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    void UpdateCoinsDisplay(int coins)
    {
        playerCoinsText.text = $"{coins} <sprite name=\"coin\">";
    }

    void OnDestroy()
    {
        
        EconomyManager.Instance.OnCoinsChanged.RemoveListener(UpdateCoinsDisplay);
    }
}