using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    /// <summary>
    /// Scriptable Object that manages the data of the player's inventory
    /// </summary>
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        private List<InventoryItem> inventoryItems;
        [field: SerializeField]
        private int size { get; set; } = 24;
        [field: SerializeField]
        private float limitWeight { get; set; } = 100;

        private float accWeight { get; set; } = 0;

        public void Init()
        {
            accWeight = 0;

            inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < size; ++i)
            {
                inventoryItems.Add(new InventoryItem());
            }
        }

        public List<InventoryItem> getAllDeteriorableItems()
        {
            return inventoryItems.FindAll(item =>
                !item.isEmpty &&
                item.item.GetType().IsSubclassOf(typeof(DeteriorableItem))
                );
        }

        /// <summary>
        /// If the new item doesn't surpass the weight limit and the inventory has a empty slot
        /// adds the item to the first empty slot
        /// Returns true if succeded, otherwise false
        /// </summary>
        /// <param name="item">Item to be added</param>
        public bool AddItem(Item item)
        {
            if (CanAddItem(item) && !IsInventoryFull())
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    if (inventoryItems[i].isEmpty)
                    {
                        inventoryItems[i].setItem(item);
                        accWeight += item.weight;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns dictionary where the key is the inventory position and the value is the <see cref="InventoryItem"/>
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, InventoryItem> GetInventoryItemsWithSlotPosition()
        {
            Dictionary<int, InventoryItem> items = new Dictionary<int, InventoryItem>();
            for (int i = 0; i < inventoryItems.Count; ++i)
            {
                if (!inventoryItems[i].isEmpty) items[i] = inventoryItems[i];
            }
            return items;
        }

        public void ReplaceItem(Item itemToReplace, Item newItem)
        {
            int index = FindItemIndex(itemToReplace);
            Remove(index);
            inventoryItems[index].setItem(newItem);
        }

        public InventoryItem GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        public Item FindItem(Item itemToFind)
        {
            return inventoryItems.Find(item => item.item.Equals(itemToFind)).item;
        }
        public int FindItemIndex(Item itemToFind)
        {
            return inventoryItems.FindIndex(item => item.item.Equals(itemToFind));
        }

        public int Remove(Item itemToFind)
        {
            int index = inventoryItems.FindIndex(item => item.item.Equals(itemToFind));
            Remove(index);
            return index;
        }

        public void Remove(int index)
        {
            if (index != -1)
            {
                inventoryItems[index].setItem(null);
            }
        }

        private bool CanAddItem(Item item) => item.weight + accWeight <= limitWeight;
        private bool IsInventoryFull() => inventoryItems.FindIndex(item => item.isEmpty) == -1;

        public int getSize() => size;

    }

    /// <summary>
    /// Class that represents the Inventary Item or Slot. It can have an <see cref="Item"/>.
    /// </summary>
    public class InventoryItem
    {
        public Item item;
        public bool isEmpty => item == null;

        public InventoryItem()
        {
            this.item = null;
        }

        public InventoryItem(Item item)
        {
            this.item = item;
        }

        public void setItem(Item item)
        {
            this.item = item;
        }
    }
}
