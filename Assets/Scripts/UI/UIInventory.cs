using Inventory.Model;
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
        private MouseFollower mouseFollower;

        [SerializeField]
        private TMPro.TextMeshProUGUI textCurrentWeight;
        [SerializeField]
        private TMPro.TextMeshProUGUI textLimitWeight;

        public event Action<int> OnItemAction, OnStartDragging, OnItemHoverStart, OnItemHoverEnd, OnItemRemoval;

        public event Action<int, int> OnSwapItems;

        private int currentlyDraggedItemIndex = -1;

        private void Awake()
        {
            mouseFollower.Hide(); 
        }

        /// <summary>
        /// Creates <paramref name="inventorySize"/> inventory slots and registers its <see cref="UIItem"/> events
        /// </summary>
        /// <param name="inventorySize"></param>
        public void InitUIInventory(int inventorySize, float limitWeight)
        {
            textLimitWeight.text = limitWeight.ToString();

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
                uiItem.OnItemHoverStart += HandleItemHoverStart;
                uiItem.OnItemHoverEnd += HandleItemHoverEnd;
            }
        }

        public void HandleItemDropOnTrash()
        {
            OnItemRemoval?.Invoke(currentlyDraggedItemIndex);
        }

        private void HandleItemHoverEnd(UIItem item)
        {
            OnItemHoverEnd?.Invoke(uiItems.FindIndex(i => i.GetInstanceID() == item.GetInstanceID()));
        }

        private void HandleItemHoverStart(UIItem item)
        {
            OnItemHoverStart?.Invoke(uiItems.FindIndex(i => i.GetInstanceID() == item.GetInstanceID()));
        }

        private void HandleEndDrag(UIItem item)
        {
            ResetDraggedItem();
        }

        private void HandleSwapItems(UIItem item)
        {
            int index = uiItems.FindIndex(i => i.GetInstanceID() == item.GetInstanceID());
            if (index == -1) return;
            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
        }

        private void HandleBeginDrag(UIItem item)
        {
            int index = uiItems.FindIndex(i => i.GetInstanceID() == item.GetInstanceID());
            if (index == -1) return;
            currentlyDraggedItemIndex = index;
            mouseFollower.SetImage(item.GetSprite());
            mouseFollower.Show();
            OnStartDragging?.Invoke(index);
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Hide();
            currentlyDraggedItemIndex = -1;
        }

        private void HandleItemAction(UIItem item)
        {
            OnItemAction?.Invoke(uiItems.FindIndex(i => i.GetInstanceID() == item.GetInstanceID()));
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
        public void AddNewItem(Sprite image, float accWeight)
        {
            textCurrentWeight.text = accWeight.ToString();
            UpdateSlot(uiItems.FindIndex(i => i.isEmpty()), image);
        }

        /// <summary>
        /// Removes from UI the first <see cref="Item"/> of <paramref name="item"/> from the inventory
        /// and returns the position of the slot
        /// </summary>
        /// <param name="item">Item to remove</param>
        public void RemoveItem(UIItem item, float accWeight)
        {
            textCurrentWeight.text = accWeight.ToString();
            UpdateSlot(uiItems.FindIndex(i => i.GetInstanceID() == item.GetInstanceID()), null);
        }

        /// <summary>
        /// Removes from UI the item that is at position <paramref name="slot"/>
        /// </summary>
        /// <param name="slot">Index of the slot from <see cref="uiItems"/></param>
        public void RemoveItem(int slot, float accWeight)
        {
            textCurrentWeight.text = accWeight.ToString();
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
