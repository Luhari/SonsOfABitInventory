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
        public int m_size { get; private set; } = 24;
        public float m_limitWeight { get; private set; } = 100;
        public float m_accWeight { get; private set; } = 0;
        public float m_coins { get; private set; } = 0;

        public event Action<float> OnAccWeightUpdated, OnCoinsUpdated;
        public event Action<string> OnActionPerformed;

        public void Init()
        {
            m_accWeight = 0;
            m_coins = 0;

            inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < m_size; ++i)
            {
                inventoryItems.Add(new InventoryItem());
            }
        }

        public List<InventoryItem> GetAllDeteriorableItems()
        {
            return inventoryItems.FindAll(item =>
                !item.isEmpty &&
                item.m_item is DeteriorableItem
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
                    InventoryItem inventoryItem = inventoryItems[i];
                    if (inventoryItem.isEmpty)
                    {
                        inventoryItem.setItem(item);
                        m_accWeight += item.m_weight;
                        OnAccWeightUpdated?.Invoke(m_accWeight);
                        if (item is IConsumable)
                        {
                            (item as IConsumable).action.OnActionPerformed += HandleOnActionPerformed;
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public bool AddItem(Item item, int index)
        {
            if (CanAddItem(item) && !IsInventoryFull())
            {
                InventoryItem inventoryItem = inventoryItems[index];
                if (inventoryItem.isEmpty)
                {
                    inventoryItem.setItem(item);
                    m_accWeight += item.m_weight;
                    OnAccWeightUpdated?.Invoke(m_accWeight);
                    if (item is IConsumable)
                    {
                        (item as IConsumable).action.OnActionPerformed += HandleOnActionPerformed;
                    }
                    return true;
                }
            }
            return false;
        }

        private void HandleOnActionPerformed(string obj)
        {
            OnActionPerformed?.Invoke(obj);
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
                InventoryItem inventoryItem = inventoryItems[i];
                if (!inventoryItem.isEmpty) items[i] = inventoryItem;
            }
            return items;
        }

        public void ReplaceItem(Item itemToReplace, Item newItem)
        {
            int index = FindItemIndex(itemToReplace);
            Remove(index);
            AddItem(newItem, index);
        }

        public InventoryItem GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        public Item FindItem(Item itemToFind)
        {
            return inventoryItems.Find(item => item.m_item.Equals(itemToFind)).m_item;
        }
        public int FindItemIndex(Item itemToFind)
        {
            return inventoryItems.FindIndex(item => item.m_item.Equals(itemToFind));
        }

        public int Remove(Item itemToFind)
        {
            int index = inventoryItems.FindIndex(item => item.m_item.Equals(itemToFind));
            Remove(index);
            return index;
        }

        public void Remove(int index)
        {
            if (index == -1) return;
            InventoryItem inventoryItem = inventoryItems[index];

            m_accWeight -= inventoryItem.m_item.m_weight;
            OnAccWeightUpdated?.Invoke(m_accWeight);
            inventoryItem.setItem(null);
        }

        public bool Sell(Item itemToFind)
        {
            int index = inventoryItems.FindIndex(item => item.m_item.Equals(itemToFind));
            return Sell(index);
        }

        public bool Sell(int index)
        {
            if (index == -1) return false;

            InventoryItem inventoryItem = inventoryItems[index];

            if (inventoryItem.m_item is Weapon ||
                inventoryItem.m_item is ResourceItem)
            {
                m_accWeight -= inventoryItem.m_item.m_weight;
                OnAccWeightUpdated?.Invoke(m_accWeight);
                m_coins += inventoryItem.m_item.m_marketValue;
                OnCoinsUpdated?.Invoke(m_coins);
                inventoryItem.setItem(null);

                return true;
            }

            return false;
        }

        private bool CanAddItem(Item item) => item.m_weight + m_accWeight <= m_limitWeight;
        private bool IsInventoryFull() => inventoryItems.FindIndex(item => item.isEmpty) == -1;


        public void SwapItems(int index1, int index2)
        {
            InventoryItem item1 = inventoryItems[index1];

            InventoryItem item2 = inventoryItems[index2];
            
            Item aux = item2.m_item;

            item2.setItem(item1.m_item);
            item1.setItem(aux);
        }
    }

    /// <summary>
    /// Class that represents the Inventary Item or Slot. It can have an <see cref="Item"/>.
    /// </summary>
    public class InventoryItem
    {
        public Item m_item;
        public bool isEmpty => m_item == null;

        public InventoryItem()
        {
            m_item = null;
        }

        public InventoryItem(Item item)
        {
            m_item = item;
        }

        public void setItem(Item item)
        {
            m_item = item;
        }
    }
}
