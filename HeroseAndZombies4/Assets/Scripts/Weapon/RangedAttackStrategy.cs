using UnityEngine;

public class RangedAttackStrategy : IWeaponAttackStrategy
{
    public bool CanAttack(Weapon weapon)
    {
        return weapon.stats.currentDurability > 0 || weapon.stats.infiniteDurability;
    }

    public void Attack(Weapon weapon)
    {
        Vector3 origin = weapon.firePoint.position;
        Vector3 direction = weapon.firePoint.forward;
        
        if (Physics.Raycast(origin, direction, out RaycastHit hit, weapon.stats.range))
        {
            hit.collider.GetComponent<IDamageable>()?.TakeDamage(weapon.CalculateDamage());
            PlayImpactEffects(hit.point);
        }
        
        DrawProjectileLine(weapon);
        PlayFireEffects(weapon);
    }

    void DrawProjectileLine(Weapon weapon)
    {
        weapon.lineRenderer.SetPosition(0, weapon.firePoint.position);
        weapon.lineRenderer.SetPosition(1, weapon.firePoint.position + weapon.firePoint.forward * weapon.stats.range);
        weapon.Invoke("ClearRay", 0.1f);
    }

    void PlayFireEffects(Weapon weapon)
    {
        AudioManager.Instance.Play("GunShot");
        VFXManager.Instance.PlayEffect("MuzzleFlash", weapon.firePoint.position);
    }

    void PlayImpactEffects(Vector3 position)
    {
        VFXManager.Instance.PlayEffect("BulletImpact", position);
    }
}