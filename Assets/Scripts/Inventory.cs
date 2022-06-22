using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to manage Player Inventory
/// </summary>
public class Inventory : MonoBehaviour
{
    public List<Item> inventoryItems = new List<Item>();

    [SerializeField]
    private ItemFactory itemFactory;
    [SerializeField]
    private UIInventory uiInventory;
    [SerializeField]
    private UIAddItemMenu uiAddItemMenu;
    [SerializeField]
    private float limitWeight = 100;

    /// <summary>
    /// Accumulated weight of the items in <see cref="inventoryItems"/>
    /// </summary>
    private float accWeight;

    private void Awake()
    {
        accWeight = 0;

        uiAddItemMenu.OnAddItem += AddItem;
    }

    /// <summary>
    /// Generates an <see cref="Item"/> of <see cref="ItemId"/> <paramref name="id"/>
    /// and adds it to the inventory
    /// </summary>
    /// <param name="id">Id of the item</param>
    public void AddItem(ItemId id)
    {
        Item item = itemFactory.generateById(id);
        AddItem(item);
    }

    /// <summary>
    /// Adds <paramref name="item"/> to the inventory
    /// If <see cref="limitWeight"/> is reached, logs out a message and doesn't add the item
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item)
    {
        if (canAddItem(item))
        {
            inventoryItems.Add(item);
            uiInventory.AddNewItem(item);
        }
        else
        {
            Debug.Log("Can't add more items, weight limit reached");
        }
    }

    /// <summary>
    /// Find in the inventory the first <see cref="Item"/> of <see cref="ItemId"/> <paramref name="id"/>
    /// </summary>
    /// <param name="id">Id of the item</param>
    /// <returns></returns>
    public Item FindItem(ItemId id)
    {
        return inventoryItems.Find(item => item.id == id);
    }

    /// <summary>
    /// Removes the first <see cref="Item"/> of <see cref="ItemId"/> <paramref name="id"/>
    /// </summary>
    /// <param name="id">Id of the item</param>
    public void RemoveItem(ItemId id)
    {
        Item item = FindItem(id);
        if (item != null)
        {
            inventoryItems.Remove(item);
            uiInventory.RemoveItem(item);
        }
    }

    /// <summary>
    /// Returns true if adding <paramref name="item"/> wouldn't surpass in weight the <see cref="limitWeight"/>,
    /// otherwise returns false
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool canAddItem(Item item)
    {
        return accWeight + item.weight <= limitWeight;
    }

}
