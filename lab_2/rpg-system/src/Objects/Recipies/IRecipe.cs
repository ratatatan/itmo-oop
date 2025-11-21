namespace RpgSystem.Objects.Recipes;

using RpgSystem.Objects.Items;

public interface IRecipe
{
    IItem Combine(IItem item1, IItem item2);
}
