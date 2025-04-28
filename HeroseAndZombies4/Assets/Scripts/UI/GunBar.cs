using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image weaponIcon;
    [SerializeField] private Slider durabilitySlider;
    [SerializeField] private TMP_Text weaponNameText;
    [SerializeField] private TMP_Text ammoText;

    public void UpdateWeaponInfo(Weapon weapon)
    {
        weaponIcon.sprite = weapon.weaponIcon;
        weaponNameText.text = weapon.stats.weaponName;
        
        if (weapon.weaponType == Weapon.WeaponType.Ranged)
        {
            ammoText.gameObject.SetActive(true);
            ammoText.text = $"{weapon.stats.currentAmmo}/{weapon.stats.maxAmmo}";
        }
        else
        {
            ammoText.gameObject.SetActive(false);
        }

        if (!weapon.stats.infiniteDurability)
        {
            durabilitySlider.gameObject.SetActive(true);
            durabilitySlider.maxValue = weapon.stats.maxDurability;
            durabilitySlider.value = weapon.stats.currentDurability;
        }
        else
        {
            durabilitySlider.gameObject.SetActive(false);
        }
    }
}