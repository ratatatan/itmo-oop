namespace RpgSystem.Objects.Items;

public abstract class Weapon : IItem
{
    public string Name { get; }
    public int Damage { get; }

    protected Weapon(string name, int damage)
    {
        Name = name;
        Damage = damage;
    }

    public abstract void Attack(Player target);

    public abstract void Use(Player player);
}
