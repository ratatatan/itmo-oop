namespace RpgSystem.Testing;

using RpgSystem.Objects;
using RpgSystem.Objects.Items;
using RpgSystem.Objects.Recipes;

public class InventoryTests
{
    [Fact]
    public void AddItem_ShouldContainItem()
    {
        var player = new Player("Test", 100);
        var inv = new Inventory(player);
        var sword = new Sword("Sword", 10);
        inv.AddItem(sword);

        Assert.Contains(sword, inv.Items);
    }

    [Fact]
    public void RemoveItem_ShouldNotContainItem()
    {
        var player = new Player("Test", 100);
        var inv = new Inventory(player);
        var shield = new Shield("Shield", 5);
        inv.AddItem(shield);
        inv.RemoveItem(shield);

        Assert.DoesNotContain(shield, inv.Items);
    }

    [Fact]
    public void UseItem_PotionShouldHealPlayerAndRemoveFromInventory()
    {
        var player = new Player("Test", 50);
        var inv = new Inventory(player);
        var potion = new Potion("Healing", new HealEffect(20));
        inv.AddItem(potion);

        player.TakeDamage(40);
        inv.UseItem(potion);

        Assert.True(player.HP > 10);
        Assert.DoesNotContain(potion, inv.Items);
    }

    [Fact]
    public void EquipItem_ShouldSetEquippedWeaponAndArmor()
    {
        var player = new Player("Test", 100);
        var inv = new Inventory(player);
        var sword = new Sword("Sword", 10);
        var shield = new Shield("Shield", 5);
        inv.AddItem(sword);
        inv.AddItem(shield);

        inv.EquipItem(sword);
        inv.EquipItem(shield);

        Assert.Equal(sword, player.EquippedWeapon);
        Assert.Equal(shield, player.EquippedArmor);
    }

    [Fact]
    public void CombineItems_WithRecipe_WorksAndRemovesOldItems()
    {
        var player = new Player("Test", 100);
        var inv = new Inventory(player);
        var sword1 = new Sword("SwordA", 5);
        var sword2 = new Sword("SwordB", 7);
        var recipe = new SwordUpgradeRecipe();

        inv.AddItem(sword1);
        inv.AddItem(sword2);

        inv.CombineItems(sword1, sword2, recipe);

        Assert.Contains(inv.Items, i => i.Name.Contains("SwordA+SwordB"));

        Assert.DoesNotContain(sword1, inv.Items);
        Assert.DoesNotContain(sword2, inv.Items);
    }
}
