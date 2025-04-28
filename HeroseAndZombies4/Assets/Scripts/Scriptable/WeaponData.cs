using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Game Data/Weapon")]
public class WeaponData : ScriptableObject
{
    [Header("Basic Settings")]
    public string weaponName;
    public GameObject prefab;
    public Sprite icon;
    public Weapon.WeaponType weaponType;

    [Header("Combat Stats")]
    public int baseDamage = 10;
    public float attackRate = 0.5f;
    public int maxDurability = 100;
    public bool infiniteDurability = false;

    [Header("Ranged Settings")]
    public int maxAmmo = 30;
    public float range = 20f;
    public Color projectileColor = Color.red;

    [Header("Melee Settings")]
    public float attackRadius = 2f;
    public float attackAngle = 90f;

    [Header("Upgrades")]
    public UpgradePath[] upgradePaths;
}

[System.Serializable]
public class UpgradePath
{
    public string pathName;
    public float damageMultiplier = 1.2f;
    public float speedMultiplier = 0.9f;
    public int durabilityBonus = 20;
}