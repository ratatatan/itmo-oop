namespace RpgSystem.Objects.Items;

public class Sword : Weapon
{
    public Sword(string name, int damage)
        : base(name, damage) { }

    public override void Use(Player target) => target.EquipWeapon(this);

    public override void Attack(Player target) => target.TakeDamage(Damage);
}
