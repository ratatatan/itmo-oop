namespace RpgSystem.Objects;

using RpgSystem.Objects.Items;
using RpgSystem.Objects.Recipes;

public class Inventory
{
    private readonly List<IItem> _items = new List<IItem>();
    private readonly Player _owner;

    public List<IItem> Items => _items;

    public Inventory(Player owner)
    {
        _owner = owner;
    }

    public void AddItem(IItem item)
    {
        _items.Add(item);
        Console.WriteLine($"Добавлено в инвентарь {_owner.Name}: {item.Name}");
    }

    public void RemoveItem(IItem item)
    {
        _items.Remove(item);
        Console.WriteLine($"Удалено из инвентаря {_owner.Name}: {item.Name}");
    }

    public void ShowInventory()
    {
        Console.WriteLine($"Инвентарь игрока {_owner.Name}:");
        foreach (var item in _items)
        {
            Console.WriteLine($"- {item.Name} ({item.GetType().Name})");
        }
    }

    public void UseItem(IItem item)
    {
        if (_items.Contains(item))
        {
            item.Use(_owner);
            Console.WriteLine($"Игрок {_owner.Name} использует {item.Name}");
            if (item is Potion)
                RemoveItem(item);
        }
    }

    public void EquipItem(IItem item)
    {
        if (!_items.Contains(item))
        {
            Console.WriteLine("Предмет не найден в инвентаре!");
            return;
        }

        switch (item)
        {
            case Weapon weapon:
                _owner.EquipWeapon(weapon);
                break;
            case Armor armor:
                _owner.EquipArmor(armor);
                break;
            default:
                Console.WriteLine("Этот предмет нельзя экипировать.");
                break;
        }
    }

    public void CombineItems(IItem item1, IItem item2, IRecipe strategy)
    {
        if (!_items.Contains(item1) || !_items.Contains(item2))
        {
            Console.WriteLine("Нет нужных предметов для объединения!");
            return;
        }
        try
        {
            var result = strategy.Combine(item1, item2);
            RemoveItem(item1);
            RemoveItem(item2);
            AddItem(result);
            Console.WriteLine($"Объединено. Получен предмет: {result.Name}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка при объединении: {e.Message}");
        }
    }
}
