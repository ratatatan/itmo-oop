namespace UniSystem;

public abstract class Person
{
    private static uint _idCounter = 0;

    public uint ID { get; }
    public string Name { get; protected set; }

    protected Person(string name)
    {
        ID = _idCounter++;
        Name = name;
    }

    public override string ToString()
    {
        return $"{GetType().Name} #{ID}: {Name}";
    }
}
