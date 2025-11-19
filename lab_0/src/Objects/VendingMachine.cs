// ✨ https://youtu.be/rH6dB_nylt0

public class VendingMachine
{
    private uint _credit = 0;
    private CoinPile _coins = new();
    private ItemShelf _items = new();

    public VendingMachine(ItemShelf items, CoinPile coins)
    {
        _items = items;
        coins = _coins;
    }

    // Загрузка монет для сдачи. Интерфейс для обслуживающего персонала.
    public void LoadCoins(CoinPile coins)
    {
        _coins.Add(coins);
        Console.WriteLine($"В автомат было загружено {coins}\nТекущий счёт: {_coins}\n");
    }

    // Загрузка товаров. Интерфейс для обслуживающего персонала.
    public void LoadItems(Item item, uint count)
    {
        _items.StockUp(item, count);
        Console.WriteLine($"Автомат был пополнен:\n{item}, {count}шт");
        Console.WriteLine($"На данный момент автомат содержит:\n{_items}");
    }

    // Загрузка товаров. Интерфейс для обслуживающего персонала.
    public void LoadItems(ItemShelf items)
    {
        _items.StockUp(items);
        Console.WriteLine($"Автомат был пополнен:\n{items}");
        Console.WriteLine($"На данный момент автомат содержит:\n{_items}");
    }

    // Ало бизнес да да деньги. Интерфейс для обслуживающего персонала.
    public CoinPile DumpCoins()
    {
        CoinPile to_return = new(_coins);
        _coins = new();
        return to_return;
    }

    // Просмотр товаров. Интерфейс для пользователей.
    public ItemShelf GetItems()
    {
        return _items;
    }

    // Вывод баланса пользователя. Интерфейс для пользователей.
    public uint GetBalance()
    {
        return _credit;
    }

    // Вставка монет. Интерфейс для пользователей.
    public void InsertCoins(CoinPile coins)
    {
        _coins.Add(coins);
        _credit += coins.Total();
        Console.WriteLine($"Ваш текущий баланс: {_credit}. Не желаете ли чего выбрать?");
    }

    // Покупка товаров. Интерфейс для пользователей.
    public Item BuyItem(String itemName)
    {
        Item item = _items.Select(itemName);

        if (_credit < item.Price)
            throw new TransactionException("У вас не хватает кредитов, чтобы купить данный товар. Внесите больше монет.");

        _credit -= item.Price;

        _items.Discard(item);
        Console.WriteLine($"Поздравляю с покупкой \"{item}\"! Ваш баланс: {GetBalance()}.");
        return item;
    }

    // Завершение работы. Интерфейс для пользователей.
    public CoinPile Done()
    {
        Console.WriteLine("Хорошего дня! Не забудьте сдачу!");
        return _coins.Subtract(_credit);
    }

}
