public class ItemShelf
{
    private Dictionary<Item, uint> _shelf = new();

    public ItemShelf() { }

    public ItemShelf(Dictionary<Item, uint> shelf)
    {
        _shelf = shelf;
    }

    public void StockUp(Item item, uint count = 1)
    {
        if (!_shelf.Keys.Contains(item))
            _shelf[item] = count;
        else
            _shelf[item] += count;
    }

    public void StockUp(ItemShelf items)
    {
        foreach (var pos in items._shelf)
        {
            if (!_shelf.ContainsKey(pos.Key))
                _shelf[pos.Key] = pos.Value;
            else
                _shelf[pos.Key] += pos.Value;
        }
    }

    public Item Select(string itemName)
    {
        if (!_shelf.Keys.Any(T => T.Name == itemName))
            throw new ItemException("Извините, такой вещи нет.");

        Item item = _shelf.Keys.First(T => T.Name == itemName);

        if (_shelf[item] == 0)
            throw new ItemException("Извините, эту вещь раскупили.");

        return item;
    }

    public void Discard(Item item, uint count = 1)
    {
        if (!_shelf.ContainsKey(item))
            throw new ItemException("Попытка выдать несуществующую вещь.");

        if (_shelf[item] == 0)
            throw new ItemException("Попытка выдать вещь, которой сейчас нет.");

        _shelf[item] -= count;
        if (_shelf[item] == 0)
            _shelf.Remove(item);
    }

    public override string ToString()
    {
        string str = (_shelf.Keys.Count() == 0 ? "0 позиций." : $"{_shelf.Keys.Count()} позиций:");
        foreach (Item item in _shelf.Keys)
        {
            str += $"\n- {item.Name} ({_shelf[item]}шт): {item.Price} кредитов";
        }
        return str;
    }
}
