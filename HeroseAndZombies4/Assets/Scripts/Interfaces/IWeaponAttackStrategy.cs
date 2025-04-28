public interface IWeaponAttackStrategy
{
    bool CanAttack(Weapon weapon);
    void Attack(Weapon weapon);
}