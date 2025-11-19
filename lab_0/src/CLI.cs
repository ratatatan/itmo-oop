public static class CLI
{
    private static bool _adminAccess = false;

    private static string _adminPassword = "admin";

    private static string _helpString =
@"Команды пользователя:
help - показывает это сообщение;
status - показывает ваши текущие нажитки;
balance - показывает количество кредитов, внесенное в автомат;
list - показывает товары, которые можно купить;
insert {1} {2} {5} {10} - позволяет внести в автомат монеты;
buy {имяТовара} - позволяет купить товар из автомата;
done - прервать шоппинг;
admin {пароль | off} - дает/отбирает права админа;
exit - завершить сессию.

Команды администратора:
load_coins {1} {2} {5} {10} - загрузить монетки в автомат;
dump_coins - получить монетки с автомата;
load_items {имя} {количество} - загрузить товары в автомат;
";

    public static void Launch(VendingMachine vm, CoinPile wallet, ItemShelf inv)
    {
        string input = "";
        Console.WriteLine(_helpString);

        while (input != "exit")
        {
            Console.Write((_adminAccess ? "[admin] " : "[user] "));
            input = Console.ReadLine().Trim();
            string[] command = input == null ? ["", ""] : input.Split(" ");
            string directive = command[0];
            string[] args = command[1..];

            switch (directive)
            {
                case "help":
                    Console.WriteLine(_helpString);
                    break;

                case "status":
                    Console.WriteLine($"На данный момент у вас {wallet}\nВещей - {inv}");
                    break;

                case "balance":
                    Console.WriteLine($"Текущий баланс: {vm.GetBalance()}");
                    break;

                case "list":
                    Console.WriteLine($"На полках виднеется {vm.GetItems()}");
                    break;

                case "buy":
                    Item bought = new();
                    try
                    {
                        bought = vm.BuyItem(args[0]);
                    }
                    catch (TransactionException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    inv.StockUp(bought);
                    break;

                case "insert":
                    try
                    {
                        List<uint> coins = new();
                        try
                        {
                            foreach (var arg in args)
                            {
                                coins.Add(UInt32.Parse(arg));
                            }

                            if (coins.Count < 4)
                                throw new FormatException();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Неверный формат ввода.");
                            input = "";
                            break;
                        }

                        CoinPile to_insert = wallet.Subtract(new CoinPile(coins[0], coins[1], coins[2], coins[3]));
                        vm.InsertCoins(to_insert);
                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(e.Message);
                    }
                    break;

                case "done":
                    CoinPile spare = vm.Done();
                    wallet.Add(spare);
                    Console.WriteLine($"Вам вернули {spare}");
                    break;

                case "admin":
                    if (args.Length == 0)
                    {
                        Console.WriteLine("Повторите команду вместе с паролем.");
                        break;
                    }
                    if (args[0] == _adminPassword)
                    {

                        Console.WriteLine("Режим администратора включен.");
                        _adminAccess = true;
                    }
                    else if (args[0] == "off")
                    {
                        Console.WriteLine("Режим администратора отключен.");
                        _adminAccess = false;
                    }
                    else
                    {
                        Console.WriteLine("Пароль неверный.");
                    }
                    break;

                case "load_coins":
                    if (!_adminAccess)
                    {
                        Console.WriteLine("У вас недостаточно прав для этого действия!");
                        break;
                    }
                    try
                    {
                        List<uint> coins = new();
                        try
                        {
                            foreach (string arg in args)
                            {
                                coins.Add(UInt32.Parse(arg));
                            }

                            if (coins.Count < 4)
                                throw new FormatException();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Неверный формат ввода.");
                            input = "";
                            break;
                        }

                        CoinPile to_insert = wallet.Subtract(new CoinPile(coins[0], coins[1], coins[2], coins[3]));
                        vm.LoadCoins(to_insert);
                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(e.Message);
                    }
                    break;

                case "load_items":
                    if (!_adminAccess)
                    {
                        Console.WriteLine("У вас недостаточно прав для этого действия!");
                        break;
                    }
                    string itemName = args[0];
                    uint count = UInt32.Parse(args[1]);

                    Item item = new();
                    try
                    {
                        item = inv.Select(itemName);
                        inv.Discard(item, count);
                        vm.LoadItems(item, count);
                    }
                    catch (ItemException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;

                case "dump_coins":
                    if (!_adminAccess)
                    {
                        Console.WriteLine("У вас недостаточно прав для этого действия!");
                        break;
                    }

                    CoinPile profit = vm.DumpCoins();
                    wallet.Add(profit);
                    Console.WriteLine($"Вы получили {profit}");
                    break;

                case "":
                case "exit":

                    break;

                default:
                    Console.WriteLine("Эта команда неверная.");
                    break;
            }
        }


    }

}
