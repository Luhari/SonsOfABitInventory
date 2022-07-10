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

        private bool m_showingItemTooltip = false;

        public event Action<InventoryItem> OnUseItem;

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
            inventoryData.OnAccWeightUpdated += HandleOnAccWeightUpdated;
            inventoryData.OnCoinsUpdated += HandleOnCoinsUpdated;
        }

        private void HandleOnCoinsUpdated(float coins)
        {
            uiInventory.UpdateCoins(coins);
        }

        private void HandleOnAccWeightUpdated(float accWeight)
        {
            uiInventory.UpdateAccWeight(accWeight);
        }

        private void PrepareDeteriorationTimerController()
        {
            deteriorationTimerController = GetComponent<DeteriorationTimerController>();
            deteriorationTimerController.OnDeterioration += HandleItemDeterioration;
        }

        public void OnForceNextLevelDeteriorationClick()
        {
            foreach (InventoryItem item in inventoryData.GetAllDeteriorableItems())
            {
                HandleItemDeterioration(item.m_item as DeteriorableItem);
            }
        }

        private void HandleItemDeterioration(DeteriorableItem item)
        {
            if (item.DeteriorOneLevel())
            {
                deteriorationTimerController.AddToTrack(item);
                uiItemTooltip.SetInfo(item.m_texture, item.m_name, item.m_weight, item.m_marketValue);
                uiItemTooltip.SetDeteriorationInfo(item.m_deteriorationLevel);
                uiInventory.UpdateSlot(inventoryData.FindItemIndex(item), item.m_texture);
            }
            else
            {
                // in case of a consumable reaching the max level deterioration, replace it with a trash item
                if (item is ConsumableItem)
                {
                    TrashItem trash = itemFactory.GenerateById(ItemId.TRASH) as TrashItem;
                    trash.SetWeight(item.m_weight);

                    inventoryData.ReplaceItem(item, trash);
                    uiInventory.UpdateSlot(inventoryData.FindItemIndex(trash), trash.m_texture);
                    uiItemTooltip.SetInfo(trash.m_texture, trash.m_name, trash.m_weight, trash.m_marketValue);
                }

                // in case of a resource reaching the max level of deterioration, lose market value
                if (item is ResourceItem)
                {
                    (item as ResourceItem).LoseMarketValueAtDeterioring();
                    uiItemTooltip.SetInfo(item.m_texture, item.m_name, item.m_weight, item.m_marketValue);
                    uiItemTooltip.SetDeteriorationInfo(item.m_deteriorationLevel);
                }
            }
        }

        /// <summary>
        /// Inits UI Inventory with empty slots and events listners for <see cref="uiInventory"/>
        /// </summary>
        private void PrepareUI()
        {
            uiInventory.InitUIInventory(inventoryData.m_size, inventoryData.m_limitWeight);
            uiInventory.OnItemAction += HandleUseItem;
            uiInventory.OnItemHoverStart += HandleItemHoverStart;
            uiInventory.OnItemHoverEnd += HandleItemHoverEnd;
            uiInventory.OnStartDragging += HandleDragging;
            uiInventory.OnSwapItems += HandleSwapItems;
            uiInventory.OnItemRemoval += HandleItemRemoval;
            uiInventory.OnItemSold += HandleItemSold;

            uiItemTooltip.Hide();
        }

        private void HandleItemSold(int itemIndex)
        {
            if (inventoryData.Sell(itemIndex))
            {
                uiInventory.RemoveItem(itemIndex);
            }
        }

        private void HandleItemRemoval(int itemIndex)
        {
            RemoveItem(itemIndex);
        }

        private void HandleSwapItems(int index1, int index2)
        {
            inventoryData.SwapItems(index1, index2);
            uiInventory.UpdateSlot(index1, inventoryData.GetItemAt(index1).m_item?.m_texture);
            uiInventory.UpdateSlot(index2, inventoryData.GetItemAt(index2).m_item?.m_texture);
        }

        private void HandleDragging(int obj)
        {
            uiItemTooltip.Hide();
        }

        private void HandleItemHoverEnd(int itemIndex)
        {
            if (m_showingItemTooltip)
            {
                m_showingItemTooltip = false;
                uiItemTooltip.Hide();
            }
        }

        private void HandleItemHoverStart(int itemIndex)
        {
            if (!m_showingItemTooltip)
            {
                m_showingItemTooltip = true;
                uiItemTooltip.Show();
                InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);

                uiItemTooltip.SetInfo(
                    inventoryItem.m_item.m_texture,
                    inventoryItem.m_item.m_name,
                    inventoryItem.m_item.m_weight,
                    inventoryItem.m_item.m_marketValue
                    );

                if (inventoryItem.m_item is DeteriorableItem)
                {
                    uiItemTooltip.SetDeteriorationInfo((inventoryItem.m_item as DeteriorableItem).m_deteriorationLevel);
                }
                
                if (inventoryItem.m_item is Weapon)
                {
                    uiItemTooltip.SetDPSInfo((inventoryItem.m_item as Weapon).m_dps);
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

            if (inventoryItem.m_item is ConsumableItem)
            {
                ConsumableItem item = inventoryItem.m_item as ConsumableItem;
                item.PerformAction();
                deteriorationTimerController.StopTracking(item);

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
            AddItem(itemFactory.GenerateById(id));
        }

        public void AddItem(Item item)
        {
            if (inventoryData.AddItem(item))
            {
                uiInventory.AddNewItem(item.m_texture);
                if (item is DeteriorableItem)
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
