using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    /// <summary>
    /// Class to manage the UI for the Player Inventory
    /// </summary>
    public class UIInventory : MonoBehaviour
    {

        public List<UIItem> uiItems = new List<UIItem>();

        [SerializeField]
        private GameObject slotItemPrefab;
        [SerializeField]
        private Transform slotsPanelTransform;
        [SerializeField]
        public event Action<int> OnItemAction, OnStartDragging;

        public event Action<int, int> OnSwapItems;

        private void Awake()
        {

        }

        /// <summary>
        /// Creates <paramref name="inventorySize"/> inventory slots and registers its <see cref="UIItem"/> events
        /// </summary>
        /// <param name="inventorySize"></param>
        public void InitUIInventory(int inventorySize)
        {
            for (int i = 0; i < inventorySize; ++i)
            {
                var slotItemInstance = Instantiate(slotItemPrefab);
                slotItemInstance.transform.SetParent(slotsPanelTransform);
                UIItem uiItem = slotItemInstance.GetComponentInChildren<UIItem>();
                uiItems.Add(uiItem);

                uiItem.OnRightMouseBtnClick += HandleItemAction;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDropped += HandleSwapItems;
                uiItem.OnItemEndDrag += HandleEndDrag;
            }
        }

        private void HandleEndDrag(UIItem item)
        {
        }

        private void HandleSwapItems(UIItem item)
        {
            // TODO
            // OnSwapItems?.Invoke()
        }

        private void HandleBeginDrag(UIItem item)
        {
        }

        private void HandleItemAction(UIItem item)
        {
            OnItemAction?.Invoke(uiItems.FindIndex(i => i.Equals(item)));
        }

        /// <summary>
        /// Updates the texture of the slot in position <paramref name="slot"/> with the sprite of <paramref name="item"/>
        /// </summary>
        /// <param name="slot">Index of the slot from <see cref="uiItems"/></param>
        /// <param name="sprite">Item that is in the slot</param>
        public void UpdateSlot(int slot, Sprite image)
        {
            if (slot < uiItems.Count)
            {
                if (image) uiItems[slot].SetItemImage(image);
                else uiItems[slot].EmptySlot();
            }
        }

        /// <summary>
        /// Add to the UI the <paramref name="item"/> to the first slot available
        /// and returns the position of the slot
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void AddNewItem(Sprite image)
        {
            UpdateSlot(uiItems.FindIndex(i => i.isEmpty()), image);
        }

        /// <summary>
        /// Removes from UI the first <see cref="Item"/> of <paramref name="item"/> from the inventory
        /// and returns the position of the slot
        /// </summary>
        /// <param name="item">Item to remove</param>
        public void RemoveItem(UIItem item)
        {
            UpdateSlot(uiItems.FindIndex(i => i.Equals(item)), null);
        }

        /// <summary>
        /// Removes from UI the item that is at position <paramref name="slot"/>
        /// </summary>
        /// <param name="slot">Index of the slot from <see cref="uiItems"/></param>
        public void RemoveItem(int slot)
        {
           UpdateSlot(slot, null);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
