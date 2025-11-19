public struct Item
{
    public uint Price;
    public string Name;

    public Item(string name, uint price)
    {
        Name = name;
        Price = price;
    }

    public override string ToString()
    {
        return Name + $": {Price} токенов";
    }

    public override int GetHashCode()
    {
        //TODO
        return base.GetHashCode();
    }
}
