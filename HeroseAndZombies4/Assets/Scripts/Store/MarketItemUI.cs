using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketItemUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button actionButton;
    [SerializeField] private GameObject lockedOverlay;
    [SerializeField] private GameObject equippedBadge;

    [Header("Button Texts")]
    [SerializeField] private string buyText = "SATIN AL";
    [SerializeField] private string equipText = "EQUIP";
    [SerializeField] private string equippedText = "EQUIPPED";

    private MarketItem currentItem;

    void Awake()
    {
        actionButton.onClick.AddListener(OnActionButtonClick);
    }

    public void Initialize(MarketItem item)
    {
        currentItem = item;
        UpdateUI();
    }

    void UpdateUI()
    {
        itemIcon.sprite = currentItem.icon;
        itemNameText.text = currentItem.displayName;
        descriptionText.text = currentItem.description;

        if (currentItem.IsUnlocked)
        {
            lockedOverlay.SetActive(false);
            priceText.gameObject.SetActive(false);
            
            bool isEquipped = CheckIfEquipped();
            equippedBadge.SetActive(isEquipped);
            actionButton.interactable = !isEquipped;
            
            actionButton.GetComponentInChildren<TMP_Text>().text = 
                isEquipped ? equippedText : equipText;
        }
        else
        {
            lockedOverlay.SetActive(true);
            priceText.gameObject.SetActive(true);
            priceText.text = $"{currentItem.CurrentPrice} <sprite name=\"coin\">";
            actionButton.GetComponentInChildren<TMP_Text>().text = buyText;
        }
    }

    void OnActionButtonClick()
    {
        if (!currentItem.IsUnlocked)
        {
            TryBuyItem();
        }
        else
        {
            EquipItem();
        }
    }

    void TryBuyItem()
    {
        if (MarketManager.Instance.PurchaseItem(currentItem))
        {
            UpdateUI();
            AudioManager.Instance.Play("PurchaseSound");
        }
        else
        {
            // Yeterli para yok
            AudioManager.Instance.Play("ErrorSound");
            StartCoroutine(ShakeButton());
        }
    }

    void EquipItem()
    {
        PlayerEquipment.Instance.EquipItem(currentItem);
        UpdateUI();
        AudioManager.Instance.Play("EquipSound");
    }

    bool CheckIfEquipped()
    {
        switch (currentItem.type)
        {
            case MarketItem.ItemType.Weapon:
                return PlayerEquipment.Instance.CurrentWeapon == currentItem.prefab;
            case MarketItem.ItemType.Skin:
                return PlayerEquipment.Instance.CurrentSkin == currentItem.prefab;
            default:
                return false;
        }
    }

    System.Collections.IEnumerator ShakeButton()
    {
        Vector3 originalPos = actionButton.transform.localPosition;
        float shakeDuration = 0.5f;
        float shakeMagnitude = 10f;
        
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            
            actionButton.transform.localPosition = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        actionButton.transform.localPosition = originalPos;
    }
}