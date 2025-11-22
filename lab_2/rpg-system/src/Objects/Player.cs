namespace RpgSystem.Objects;

using RpgSystem.Objects.Items;

public class Player
{
    public string Name { get; }
    public int MaxHP { get; }
    private int _hp;
    public int HP => _hp;

    public Weapon? EquippedWeapon { get; private set; }
    public Armor? EquippedArmor { get; private set; }

    public bool IsAlive => _hp > 0;

    public Player(string name, int maxHp)
    {
        Name = name;
        MaxHP = maxHp;
        _hp = maxHp;
    }

    public void EquipWeapon(Weapon weapon)
    {
        EquippedWeapon = weapon;
        Console.WriteLine($"{Name} экипировал {weapon.Name} (+{weapon.Damage} к урону)");
    }
    public void EquipArmor(Armor armor)
    {
        EquippedArmor = armor;
        Console.WriteLine($"{Name} экипировал {armor.Name} (+{armor.Defense} к защите)");
    }

    public int CalculateDamage(int damage)
    {
        int physicalProtection = EquippedArmor?.Defense ?? 0;
        return Math.Max(0, damage - physicalProtection);
    }

    public void TakeDamage(int damage)
    {
        int total = CalculateDamage(damage);

        _hp -= total;
        Console.WriteLine($"{Name} получил {total} урона. HP: {_hp}/{MaxHP}");

        if (_hp <= 0)
            OnDeath();
    }

    public void Heal(int amount)
    {
        if (!IsAlive)
        {
            Console.WriteLine($"{Name} не может быть вылечен — мёртв.");
            return;
        }
        _hp += amount;
        if (_hp > MaxHP) _hp = MaxHP;
        Console.WriteLine($"{Name} исцелён на {amount} HP. HP: {_hp}/{MaxHP}");
    }


    private void OnDeath()
    {
        Console.WriteLine($"{Name} погиб!");
    }
}
