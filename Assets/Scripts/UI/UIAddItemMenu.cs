using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    /// <summary>
    /// Class that manages the UI of the buttons to add new items to the inventory
    /// </summary>
    public class UIAddItemMenu : MonoBehaviour
    {

        public event Action<ItemId> OnAddItem;

        [SerializeField]
        private ItemFactory itemFactory;

        [SerializeField]
        private GameObject addButtonPrefab;

        private void Awake()
        {
            itemFactory.OnDatabaseBuilt += BuildAddItemsMenu;
        }

        /// <summary>
        /// Once our database from <see cref="itemFactory"/> is built, create 1 button for each 
        /// unique <see cref="ItemId"/>
        /// </summary>
        /// <param name="database"></param>
        private void BuildAddItemsMenu(List<(ItemId, Sprite)> database)
        {
            foreach ((ItemId itemId, Sprite sprite) pair in database)
            {
                GameObject instance = Instantiate(addButtonPrefab);
                instance.transform.SetParent(gameObject.transform);

                Image image = instance.GetComponentsInChildren<Image>()[1];
                image.sprite = pair.sprite;

                var button = instance.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    OnAddItem?.Invoke(pair.itemId);
                });
            }
        }
    }
}
