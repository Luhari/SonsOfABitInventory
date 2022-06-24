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
        private DeteriorationTimerController deteriorationTimerController;

        [SerializeField]
        private UIAddItemMenu uiAddItemMenu;
        [SerializeField]
        private UIItemTooltip uiItemTooltip;

        public event Action<InventoryItem> OnUseItem;

        private bool showingItemTooltip = false;


        private void Awake()
        {
            PrepareUI();
            PrepareInventoryData();
            PrepareDeteriorationTimerController();

            uiAddItemMenu.OnAddItem += AddItem;
        }

        /// <summary>
        /// Inits <see cref="inventoryData"/>
        /// </summary>
        private void PrepareInventoryData()
        {
            inventoryData.Init();
        }

        private void PrepareDeteriorationTimerController()
        {
            deteriorationTimerController = GetComponent<DeteriorationTimerController>();
            deteriorationTimerController.OnDeterioration += HandleItemDeterioration;
        }

        public void OnForceNextLevelDeteriorationClick()
        {
            foreach (InventoryItem item in inventoryData.getAllDeteriorableItems())
            {
                HandleItemDeterioration(item.item as DeteriorableItem);
            }
        }

        private void HandleItemDeterioration(DeteriorableItem item)
        {
            if (item.deteriorOneLevel())
            {
                deteriorationTimerController.AddToTrack(item);
                uiItemTooltip.SetInfo(item.texture, item.name, item.weight, item.marketValue);
                uiItemTooltip.SetDeteriorationInfo(item.deteriorationLevel);
                uiInventory.UpdateSlot(inventoryData.FindItemIndex(item), item.texture);
            }
            else
            {
                // in case of a consumable reaching the max level deterioration, replace it with a trash item
                if (item.GetType() == typeof(ConsumableItem))
                {
                    TrashItem trash = itemFactory.generateById(ItemId.TRASH) as TrashItem;
                    trash.SetWeight(item.weight);

                    inventoryData.ReplaceItem(item, trash);
                    uiInventory.UpdateSlot(inventoryData.FindItemIndex(trash), trash.texture);
                    uiItemTooltip.SetInfo(trash.texture, trash.name, trash.weight, trash.marketValue);
                }

                // in case of a resource reaching the max level of deterioration, lose market value
                if (item.GetType() == typeof(ResourceItem))
                {
                    (item as ResourceItem).loseMarketValueAtDeterioring();
                    uiItemTooltip.SetInfo(item.texture, item.name, item.weight, item.marketValue);
                    uiItemTooltip.SetDeteriorationInfo(item.deteriorationLevel);
                }
            }
        }

        /// <summary>
        /// Inits UI Inventory with empty slots and events listners for <see cref="uiInventory"/>
        /// </summary>
        private void PrepareUI()
        {
            uiInventory.InitUIInventory(inventoryData.getSize());
            uiInventory.OnItemAction += HandleUseItem;
            uiInventory.OnItemHoverStart += HandleItemHoverStart;
            uiInventory.OnItemHoverEnd += HandleItemHoverEnd;
            // uiInventory.OnStartDragging += HandleDragging;
            // uiInventory.OnSwapItems += HandleSwapItems;

            uiItemTooltip.Hide();
        }

        private void HandleItemHoverEnd(int itemIndex)
        {
            if (showingItemTooltip)
            {
                showingItemTooltip = false;
                uiItemTooltip.Hide();
            }
        }

        private void HandleItemHoverStart(int itemIndex)
        {
            if (!showingItemTooltip)
            {
                showingItemTooltip = true;
                uiItemTooltip.Show();
                InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);

                uiItemTooltip.SetInfo(
                    inventoryItem.item.texture,
                    inventoryItem.item.name,
                    inventoryItem.item.weight,
                    inventoryItem.item.marketValue
                    );

                if (inventoryItem.item.GetType().IsSubclassOf(typeof(DeteriorableItem)))
                {
                    uiItemTooltip.SetDeteriorationInfo((inventoryItem.item as DeteriorableItem).deteriorationLevel);
                }
                
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
                if (item.GetType().IsSubclassOf(typeof(DeteriorableItem)))
                {
                    deteriorationTimerController.AddToTrack(item as DeteriorableItem);
                }
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
    }

}
