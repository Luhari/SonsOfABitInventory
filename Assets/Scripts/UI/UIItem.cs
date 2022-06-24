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
    public class UIItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDropHandler, IEndDragHandler
    {
        // Image child of the SlotItemPrefab that will show the texture of the item
        private Image image;

        private bool empty = true;

        public event Action<UIItem> OnRightMouseBtnClick,
            OnItemBeginDrag, OnItemDropped, OnItemEndDrag;

        public void Awake()
        {
            image = GetComponentsInChildren<Image>()[1];
            EmptySlot();
        }

        public bool isEmpty()
        {
            return empty;
        }

        public Sprite GetSprite()
        {
            return image.sprite;
        }

        /// <summary>
        /// Disable the item's image and sets <see cref="empty"/> to true
        /// </summary>
        public void EmptySlot()
        {
            image.enabled = false;
            empty = true;
        }

        /// <summary>
        /// Enables the item's image and sets <see cref="empty"/> to false
        /// </summary>
        public void SetItemImage(Sprite sprite)
        {
            image.sprite = sprite;
            image.enabled = true;
            empty = false;
        }

        /// <summary>
        /// If the <see cref="UIItem"/> is empty, does nothing. If is not empty and it has been
        /// right clicked, invokes <see cref="OnRightMouseBtnClick"/> event
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (empty) return;

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightMouseBtnClick?.Invoke(this);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (empty) return;
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
    }
}
