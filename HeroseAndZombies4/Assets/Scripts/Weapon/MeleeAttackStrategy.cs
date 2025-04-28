using UnityEngine;

public class MeleeAttackStrategy : IWeaponAttackStrategy
{
    public bool CanAttack(Weapon weapon)
    {
        return weapon.stats.currentDurability > 0 || weapon.stats.infiniteDurability;
    }

    public void Attack(Weapon weapon)
    {
        Vector3 origin = weapon.firePoint.position;
        float radius = weapon.stats.attackRadius;
        
        Collider[] hits = Physics.OverlapSphere(origin, radius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Vector3 dirToTarget = (hit.transform.position - origin).normalized;
                if (Vector3.Angle(weapon.firePoint.forward, dirToTarget) <= weapon.stats.attackAngle / 2)
                {
                    hit.GetComponent<IDamageable>()?.TakeDamage(weapon.CalculateDamage());
                }
            }
        }
        
        PlaySwingEffects(weapon);
    }

    void PlaySwingEffects(Weapon weapon)
    {
        AudioManager.Instance.Play("SwordSwing");
        VFXManager.Instance.PlayEffect("SwordTrail", weapon.firePoint.position);
    }
}