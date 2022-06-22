using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to manage the UI for an Item in a Slot of the inventory
/// </summary>
public class UIItem : MonoBehaviour
{
    /// <summary>
    /// Item of the slot
    /// </summary>
    public Item item = null;
    /// <summary>
    /// Image child of the SlotPrefab that will show the texture of the item
    /// </summary>
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        UpdateTexture(null);
    }

    /// <summary>
    /// If the slot has an item, populates <see cref="item"/> with <paramref name="item"/> 
    /// and <see cref="image"/> with <paramref name="item.texture"/>
    /// Otherwise <see cref="item"/> is null and <see cref="image"/> is transparent
    /// </summary>
    /// <param name="item">The item of the slot</param>
    public void UpdateTexture(Item item)
    {
        this.item = item;
        if (this.item != null)
        {
            image.color = Color.white;
            image.sprite = this.item.texture;
        } 
        else
        {
            image.color = Color.clear;
        }
    }
}
