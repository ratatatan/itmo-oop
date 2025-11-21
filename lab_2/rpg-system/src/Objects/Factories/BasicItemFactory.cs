namespace RpgSystem.Objects.Factories;

using RpgSystem.Objects.Items;


public class BasicItemFactory : IItemFactory
{
    public Weapon CreateWeapon() => new Sword("Простой меч", 10);
    public Armor CreateArmor() => new Shield("Простой щит", 5);
    public Potion CreatePotion() => new Potion("Малое зелье лечения", new HealEffect(10));
}
