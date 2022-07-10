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
        [SerializeField]
        private TMPro.TextMeshProUGUI m_textCurrentWeight;
        [SerializeField]
        private TMPro.TextMeshProUGUI m_textLimitWeight;
        [SerializeField]
        private TMPro.TextMeshProUGUI m_textCoins;
        private int m_currentlyDraggedItemIndex = -1;

        [SerializeField]
        private GameObject slotItemPrefab;
        [SerializeField]
        private Transform slotsPanelTransform;
        [SerializeField]
        private MouseFollower mouseFollower;

        private List<UIItem> m_uiItems = new List<UIItem>();

        public event Action<int> OnItemAction, OnStartDragging, OnItemHoverStart, OnItemHoverEnd, 
            OnItemRemoval, OnItemSold;

        public event Action<int, int> OnSwapItems;


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
            m_textLimitWeight.text = limitWeight.ToString();

            for (int i = 0; i < inventorySize; ++i)
            {
                var slotItemInstance = Instantiate(slotItemPrefab);
                slotItemInstance.transform.SetParent(slotsPanelTransform);
                UIItem uiItem = slotItemInstance.GetComponentInChildren<UIItem>();
                m_uiItems.Add(uiItem);

                uiItem.OnRightMouseBtnClick += HandleItemAction;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDropped += HandleSwapItems;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnItemHoverStart += HandleItemHoverStart;
                uiItem.OnItemHoverEnd += HandleItemHoverEnd;
            }
        }

        public void HandleItemDropOnSell()
        {
            OnItemSold?.Invoke(m_currentlyDraggedItemIndex);
        }

        public void HandleItemDropOnTrash()
        {
            OnItemRemoval?.Invoke(m_currentlyDraggedItemIndex);
        }

        private void HandleItemHoverEnd(UIItem item)
        {
            OnItemHoverEnd?.Invoke(m_uiItems.FindIndex(i => i.GetInstanceID() == item.GetInstanceID()));
        }

        private void HandleItemHoverStart(UIItem item)
        {
            OnItemHoverStart?.Invoke(m_uiItems.FindIndex(i => i.GetInstanceID() == item.GetInstanceID()));
        }

        private void HandleEndDrag(UIItem item)
        {
            ResetDraggedItem();
        }

        private void HandleSwapItems(UIItem item)
        {
            int index = m_uiItems.FindIndex(i => i.GetInstanceID() == item.GetInstanceID());
            if (index == -1) return;
            OnSwapItems?.Invoke(m_currentlyDraggedItemIndex, index);
        }

        private void HandleBeginDrag(UIItem item)
        {
            int index = m_uiItems.FindIndex(i => i.GetInstanceID() == item.GetInstanceID());
            if (index == -1) return;
            m_currentlyDraggedItemIndex = index;
            mouseFollower.SetImage(item.GetSprite());
            mouseFollower.Show();
            OnStartDragging?.Invoke(index);
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Hide();
            m_currentlyDraggedItemIndex = -1;
        }

        private void HandleItemAction(UIItem item)
        {
            OnItemAction?.Invoke(m_uiItems.FindIndex(i => i.GetInstanceID() == item.GetInstanceID()));
        }

        public void UpdateAccWeight(float value)
        {
            m_textCurrentWeight.text = value.ToString();
        }

        public void UpdateCoins(float value)
        {
            m_textCoins.text = value.ToString();
        }

        /// <summary>
        /// Updates the texture of the slot in position <paramref name="slot"/> with the sprite of <paramref name="item"/>
        /// </summary>
        /// <param name="slot">Index of the slot from <see cref="uiItems"/></param>
        /// <param name="sprite">Item that is in the slot</param>
        public void UpdateSlot(int slot, Sprite image)
        {
            if (slot < m_uiItems.Count)
            {
                if (image) m_uiItems[slot].SetItemImage(image);
                else m_uiItems[slot].EmptySlot();
            }
        }

        /// <summary>
        /// Add to the UI the <paramref name="item"/> to the first slot available
        /// and returns the position of the slot
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void AddNewItem(Sprite image)
        {
            UpdateSlot(m_uiItems.FindIndex(i => i.IsEmpty()), image);
        }

        /// <summary>
        /// Removes from UI the first <see cref="Item"/> of <paramref name="item"/> from the inventory
        /// and returns the position of the slot
        /// </summary>
        /// <param name="item">Item to remove</param>
        public void RemoveItem(UIItem item)
        {
            UpdateSlot(m_uiItems.FindIndex(i => i.GetInstanceID() == item.GetInstanceID()), null);
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
