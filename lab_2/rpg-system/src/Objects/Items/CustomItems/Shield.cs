namespace RpgSystem.Objects.Items;

public class Shield : Armor
{
    public Shield(string name, int defense)
        : base(name, defense) { }

    public override void Use(Player target) => target.EquipArmor(this);

}
