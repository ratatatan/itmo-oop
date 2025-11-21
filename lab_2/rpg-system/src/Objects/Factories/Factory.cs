namespace RpgSystem.Objects.Factories;

using RpgSystem.Objects.Items;

public interface IItemFactory
{
    Weapon CreateWeapon();
    Armor CreateArmor();
    Potion CreatePotion();
}
