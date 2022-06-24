using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// Class to manage Player Inventory
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        [SerializeField]
        private UIInventory uiInventory;
        [SerializeField]
        private InventorySO inventoryData;
        [SerializeField]
        private ItemFactory itemFactory;

        [SerializeField]
        private UIAddItemMenu uiAddItemMenu;

        public event Action<InventoryItem> OnUseItem;


        private void Awake()
        {
            PrepareUI();
            PrepareInventoryData();


            uiAddItemMenu.OnAddItem += AddItem;
        }

        /// <summary>
        /// Inits UI Inventory with empty slots and events listners for <see cref="uiInventory"/>
        /// </summary>
        private void PrepareUI()
        {
            uiInventory.InitUIInventory(inventoryData.getSize());
            uiInventory.OnItemAction += HandleUseItem;
            // uiInventory.OnStartDragging += HandleDragging;
            // uiInventory.OnSwapItems += HandleSwapItems;
        }

        /// <summary>
        /// Inits <see cref="inventoryData"/>
        /// </summary>
        private void PrepareInventoryData()
        {
            inventoryData.Init();
        }

        public void Update()
        {
            // show or hide Inventory at Key I click
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (!uiInventory.isActiveAndEnabled)
                {
                    uiInventory.Show();
                }
                else
                {
                    uiInventory.Hide();
                }
            }
        }

        public void AddItem(ItemId id)
        {
            AddItem(itemFactory.generateById(id));
        }

        public void AddItem(Item item)
        {
            if (inventoryData.AddItem(item))
            {
                uiInventory.AddNewItem(item.texture);
            }
        }

        public void RemoveItem(Item itemToFind)
        {
            Item item = inventoryData.FindItem(itemToFind);
            if (item != null)
            {
                int index = inventoryData.Remove(item);
                if (index != -1) uiInventory.RemoveItem(index);
            }
        }

        public void RemoveItem(int index)
        {
            if (index != -1)
            {
                inventoryData.Remove(index);
                if (index != -1) uiInventory.RemoveItem(index);
            }
        }

        /// <summary>
        /// <see cref="OnUseItem"/> handler, check the instance type class and runs PerformAction
        /// </summary>
        /// <param name="itemIndex"></param>
        private void HandleUseItem(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            
            if (inventoryItem.item.GetType() == typeof(ConsumableItem))
            {
                ConsumableItem item = inventoryItem.item as ConsumableItem;
                item.PerformAction();

                RemoveItem(itemIndex);
            }
        }
    }

}
