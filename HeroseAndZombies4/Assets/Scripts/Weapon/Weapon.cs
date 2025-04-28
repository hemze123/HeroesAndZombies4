using UnityEngine;
using System.Collections;

[System.Serializable]
public class Weapon : MonoBehaviour
{
    public enum WeaponType { Melee, Ranged }
    public WeaponType weaponType;
    public WeaponStats stats;
    public Transform firePoint;
    public LineRenderer lineRenderer;
    public Sprite weaponIcon;

    [Header("Upgrade Settings")]
    public int currentUpgradeLevel = 0;
    public UpgradePath[] upgradePaths;

    private IWeaponAttackStrategy attackStrategy;
    private float nextFireTime;

    void Awake()
    {
        // Strateji deseni uygulama
        attackStrategy = weaponType switch
        {
            WeaponType.Melee => new MeleeAttackStrategy(),
            WeaponType.Ranged => new RangedAttackStrategy(),
            _ => null
        };

        InitializeWeapon();
    }

    void InitializeWeapon()
    {
        if (weaponType == WeaponType.Ranged && lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.material = new Material(Shader.Find("Unlit/Color")) 
            { 
                color = stats.projectileColor 
            };
        }

        if (!stats.infiniteDurability)
            stats.currentDurability = stats.maxDurability;
    }

    public void TriggerAttack()
    {
        if (Time.time >= nextFireTime && attackStrategy.CanAttack(this))
        {
            attackStrategy.Attack(this);
            nextFireTime = Time.time + GetFireRate();
            ReduceDurability();
        }
    }

    void ReduceDurability()
    {
        if (!stats.infiniteDurability)
        {
            stats.currentDurability--;
            if (stats.currentDurability <= 0) WeaponBroken();
        }
    }

    void WeaponBroken()
    {
        Debug.Log($"{stats.weaponName} kırıldı!");
        Destroy(gameObject);
    }

    public int CalculateDamage()
    {
        float damage = stats.baseDamage * Mathf.Pow(1.2f, currentUpgradeLevel);
        return Mathf.CeilToInt(damage);
    }

    public float GetFireRate()
    {
        return stats.baseFireRate * Mathf.Pow(0.9f, currentUpgradeLevel);
    }

    public void UpgradeWeapon()
    {
        currentUpgradeLevel++;
        GameEvents.OnWeaponUpgraded?.Invoke(this, currentUpgradeLevel);
    }
}