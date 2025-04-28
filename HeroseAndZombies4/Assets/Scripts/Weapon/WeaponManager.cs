using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    [SerializeField] private Transform weaponHolder;
    [SerializeField] private List<Weapon> unlockedWeapons = new List<Weapon>();
    
    public UnityEvent<Weapon> OnWeaponChanged = new UnityEvent<Weapon>();
    private Weapon currentWeapon;
    private GameObject currentWeaponObj;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void UnlockWeapon(Weapon weaponPrefab)
    {
        if (!unlockedWeapons.Contains(weaponPrefab))
        {
            unlockedWeapons.Add(weaponPrefab);
            SaveManager.Instance.SaveInt($"weapon_{weaponPrefab.name}_unlocked", 1);
        }
    }

    public void EquipWeapon(Weapon weaponPrefab)
    {
        if (currentWeaponObj != null)
            Destroy(currentWeaponObj);

        currentWeaponObj = Instantiate(weaponPrefab.gameObject, weaponHolder);
        currentWeapon = currentWeaponObj.GetComponent<Weapon>();
        currentWeapon.transform.localPosition = Vector3.zero;
        
        GameEvents.OnWeaponChanged?.Invoke(currentWeapon);
    }

    public Weapon GetCurrentWeapon() => currentWeapon;
    public List<Weapon> GetUnlockedWeapons() => unlockedWeapons;
}