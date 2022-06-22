using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to manage the UI for the Player Inventory
/// </summary>
public class UIInventory : MonoBehaviour
{
    public List<UIItem> uiItems = new List<UIItem>();

    private int numberOfSlots = 24;
    
    void Awake()
    {
        uiItems = new List<UIItem>(GetComponentsInChildren<UIItem>());
    }

    /// <summary>
    /// Updates the texture of the slot in position <paramref name="slot"/> with the sprite of <paramref name="item"/>
    /// </summary>
    /// <param name="slot">Index of the slot from <see cref="uiItems"/></param>
    /// <param name="item">Item that is in the slot</param>
    public void UpdateSlot(int slot, Item item)
    {
        uiItems[slot].UpdateTexture(item);
    }

    /// <summary>
    /// Add to the UI the <paramref name="item"/> to the first slot available
    /// </summary>
    /// <param name="item">Item to be added</param>
    public void AddNewItem(Item item)
    {
        // TODO weight constraint
        UpdateSlot(uiItems.FindIndex(i => i.item == null), item);
    }

    /// <summary>
    /// Removes from UI the first <see cref="Item"/> of <paramref name="item"/> from the inventory
    /// </summary>
    /// <param name="item">Item to remove</param>
    public void RemoveItem(Item item)
    {
        UpdateSlot(uiItems.FindIndex(i => i.item == item), null);
    }
}
