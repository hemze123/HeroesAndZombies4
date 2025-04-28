using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponStats", menuName = "Weapons/Weapon Stats")]
public class WeaponStats : ScriptableObject
{
    [Header("Basic Info")]
    public string weaponName;
    public string description;
    public int baseDamage = 10;
    public float baseFireRate = 0.5f;

    [Header("Durability")]
    public int maxDurability = 100;
    public int currentDurability;
    public bool infiniteDurability = false;

    [Header("Ranged Settings")]
    public int maxAmmo;
    public int currentAmmo;
    public bool infiniteAmmo;
    public float range = 10f;
    public Color projectileColor = Color.red;
    public float projectileSpeed = 20f;

    [Header("Melee Settings")]
    public float attackRadius = 2f;
    public float attackAngle = 60f;

   

}