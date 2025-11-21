namespace RpgSystem.Objects.Items;

public interface IPotionEffect
{
    void Apply(Player target);
}

public class Potion : IItem
{
    public string Name { get; }
    private readonly IPotionEffect _effect;

    public Potion(string name, IPotionEffect effect)
    {
        Name = name;
        _effect = effect;
    }

    public void Use(Player target)
    {
        _effect.Apply(target);
        Console.WriteLine($"{target.Name} выпил {Name}");
    }
}

// Strategy pattern
public class HealEffect : IPotionEffect
{
    private int _healing;
    public HealEffect(int healing) => _healing = healing;

    public void Apply(Player target)
    {
        target.Heal(_healing);
        Console.WriteLine($"{target.Name} получил +{_healing}HP.");
    }
}

public class DamageEffect : IPotionEffect
{
    private int _damage;
    public DamageEffect(int damage) => _damage = damage;

    public void Apply(Player target)
    {
        target.TakeDamage(_damage);
        Console.WriteLine($"{target.Name} получил -{_damage}HP.");
    }
}
