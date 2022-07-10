using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Inventory.UI
{
    /// <summary>
    /// Class to manage the UI for an Item in a Slot of the inventory
    /// </summary>
    public class UIItem : MonoBehaviour, 
        IPointerClickHandler, IBeginDragHandler, IDropHandler, IEndDragHandler, 
        IPointerEnterHandler, IPointerExitHandler, IDragHandler
    {
        // Image child of the SlotItemPrefab that will show the texture of the item
        private Image m_image;

        private bool m_empty = true;

        public event Action<UIItem> OnRightMouseBtnClick,
            OnItemBeginDrag, OnItemDropped, OnItemEndDrag,
            OnItemHoverStart, OnItemHoverEnd;


        public void Awake()
        {
            m_image = GetComponentsInChildren<Image>()[1];
            EmptySlot();
        }

        public bool IsEmpty()
        {
            return m_empty;
        }

        public Sprite GetSprite()
        {
            return m_image.sprite;
        }

        /// <summary>
        /// Disable the item's image and sets <see cref="empty"/> to true
        /// </summary>
        public void EmptySlot()
        {
            m_image.enabled = false;
            m_empty = true;
        }

        /// <summary>
        /// Enables the item's image and sets <see cref="empty"/> to false
        /// </summary>
        public void SetItemImage(Sprite sprite)
        {
            m_image.sprite = sprite;
            m_image.enabled = true;
            m_empty = false;
        }

        /// <summary>
        /// If the <see cref="UIItem"/> is empty, does nothing. If is not empty and it has been
        /// right clicked, invokes <see cref="OnRightMouseBtnClick"/> event
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_empty) return;

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightMouseBtnClick?.Invoke(this);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (m_empty) return;
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDropped?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (m_empty) return;
            OnItemHoverStart?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (m_empty) return;
            OnItemHoverEnd?.Invoke(this);
        }

        // without it doesn't detect on begindrag...
        public void OnDrag(PointerEventData eventData)
        {
            
        }
    }
}
