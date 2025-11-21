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
        var sword = new Sword("Sword", 10);
        player.Inventory.AddItem(sword);

        Assert.Contains(sword, player.Inventory.Items); // если Сделан публичный getter для Items или через ShowInventory()
    }

    [Fact]
    public void RemoveItem_ShouldNotContainItem()
    {
        var player = new Player("Test", 100);
        var shield = new Shield("Shield", 5);
        player.Inventory.AddItem(shield);
        player.Inventory.RemoveItem(shield);

        Assert.DoesNotContain(shield, player.Inventory.Items);
    }

    [Fact]
    public void UseItem_PotionShouldHealPlayerAndRemoveFromInventory()
    {
        var player = new Player("Test", 50);
        var potion = new Potion("Healing", new HealEffect(20));
        player.Inventory.AddItem(potion);

        player.TakeDamage(40); // hp = 10
        player.Inventory.UseItem(potion);

        Assert.True(player.HP > 10);     // Был вылечен
        Assert.DoesNotContain(potion, player.Inventory.Items); // Зелье удалено после использования
    }

    [Fact]
    public void EquipItem_ShouldSetEquippedWeaponAndArmor()
    {
        var player = new Player("Test", 100);
        var sword = new Sword("Sword", 10);
        var shield = new Shield("Shield", 5);
        player.Inventory.AddItem(sword);
        player.Inventory.AddItem(shield);

        player.Inventory.EquipItem(sword);
        player.Inventory.EquipItem(shield);

        Assert.Equal(sword, player.EquippedWeapon);
        Assert.Equal(shield, player.EquippedArmor);
    }

    [Fact]
    public void CombineItems_WithRecipe_WorksAndRemovesOldItems()
    {
        var player = new Player("Test", 100);
        var sword1 = new Sword("SwordA", 5);
        var sword2 = new Sword("SwordB", 7);
        var recipe = new SwordUpgradeRecipe();

        player.Inventory.AddItem(sword1);
        player.Inventory.AddItem(sword2);

        player.Inventory.CombineItems(sword1, sword2, recipe);

        // Новый предмет появился
        Assert.Contains(player.Inventory.Items, i => i.Name.Contains("SwordA+SwordB"));
        // Оригинальные ушли
        Assert.DoesNotContain(sword1, player.Inventory.Items);
        Assert.DoesNotContain(sword2, player.Inventory.Items);
    }
}
