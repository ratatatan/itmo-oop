namespace RpgSystem.Objects.Items;

public abstract class Armor : IItem
{
    public string Name { get; }
    public int Defense { get; }

    protected Armor(string name, int defense)
    {
        Name = name;
        Defense = defense;
    }

    public abstract void Use(Player player);
}
