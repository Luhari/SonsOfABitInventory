using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ID of the item for <see cref="database"/>
/// </summary>
public enum ItemId
{
    SWORD = 0,
    BOW,
    HEALTH_POTION,
    MANA_POTION,
    ARROW,
    WOOD,
    TRASH
}
/// <summary>
/// Class that manages the generation of new items.
/// </summary>
public class ItemFactory: MonoBehaviour
{
    /// <summary>
    /// Contains all unique <see cref="Item"/>
    /// </summary>
    public List<Item> database = new List<Item>();

    public delegate void OnDatabaseBuiltDelegate(List<Item> database);
    public OnDatabaseBuiltDelegate OnDatabaseBuilt;

    [SerializeField]
    private UIAddItemMenu uiAddItemMenu;

    public void Awake()
    {
        BuildDatabase();
    }

    /// <summary>
    /// Generates all unique items into <see cref="database"/>
    /// </summary>
    void BuildDatabase()
    {
        database = new List<Item>()
        {
            new Weapon(ItemId.SWORD, "Sword", 10f, 1f, "sword", 1f),
            new Weapon(ItemId.BOW, "Bow", 1f, 1f, "bow", 1f),
            new ConsumableItem(ItemId.HEALTH_POTION, "Health Potion", 1f, "healthPotion0", 1f, 3),
            new ConsumableItem(ItemId.MANA_POTION, "Mana Potion", 1f, "manaPotion0", 1f, 3),
            new ResourceItem(ItemId.ARROW, "Arrow", 1f, "arrow0", 1f, 1f, 3),
            new ResourceItem(ItemId.WOOD, "Wood", 1f, "wood0", 1f, 1f, 3),
            new TrashItem(ItemId.TRASH, "Trash", 1f),
        };
        OnDatabaseBuilt(database);
    }

    /// <summary>
    /// Returns an <see cref="Item"/> with <see cref="ItemId"/> <paramref name="id"/> from the <see cref="database"/>
    /// </summary>
    /// <param name="id">Id of the item</param>
    /// <returns></returns>
    public Item generateById(ItemId id)
    {
        return database.Find(item => item.id == id);
    }
}
