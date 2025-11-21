namespace RpgSystem.Objects.Items;

public interface IItem
{
    string Name { get; }
    void Use(Player target);
}
