namespace RpgSystem.Objects.Recipes;

using RpgSystem.Objects.Items;

public class SwordUpgradeRecipe : IRecipe
{
    public IItem Combine(IItem item1, IItem item2)
    {
        if (item1 is Sword s1 && item2 is Sword s2)
        {
            string newName = $"{s1.Name}+{s2.Name}";
            int newDamage = s1.Damage + s2.Damage;
            return new Sword(newName, newDamage);
        }
        throw new InvalidOperationException("Можно объединять только мечи!");
    }
}
