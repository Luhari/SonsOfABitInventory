using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory
{

    /// <summary>
    /// ID of the item for <see cref="database"/>
    /// </summary>
    public enum ItemId
    {
        SWORD = 0,
        BOW,
        HEALTH_POTION,
        MANA_POTION,
        ARROW,
        WOOD,
        TRASH
    }
    /// <summary>
    /// Class that manages the generation of new items and stores a list of all uniques <see cref="Item"/>.
    /// </summary>
    public class ItemFactory: MonoBehaviour
    {
        /// <summary>
        /// Contains all unique <see cref="Item"/>
        /// </summary>
        public List<Item> database;

        public event Action<List<(ItemId, Sprite)>> OnDatabaseBuilt;

        public void Start()
        { 
            BuildDatabase();
        }

        /// <summary>
        /// Generates all unique items into <see cref="database"/> and invokes <see cref="OnDatabaseBuilt"/>
        /// </summary>
        void BuildDatabase()
        {
            database = new List<Item>()
            {
                new Weapon(ItemId.SWORD, "Sword", 10f, 1f, "sword", 1f),
                new Weapon(ItemId.BOW, "Bow", 1f, 1f, "bow", 1f),
                new ConsumableItem(ItemId.HEALTH_POTION, "Health Potion", 
                    1f, "healthPotion0", 1f, 3, new UseHealthPotion(10f)),
                new ConsumableItem(ItemId.MANA_POTION, "Mana Potion", 
                    1f, "manaPotion0", 1f, 3, new UseManaPotion(1f)),
                new ResourceItem(ItemId.ARROW, "Arrow", 1f, "arrow0", 1f, 1f, 3),
                new ResourceItem(ItemId.WOOD, "Wood", 1f, "wood0", 1f, 1f, 3),
                new TrashItem(ItemId.TRASH, "Trash", 1f),
            };

            List<(ItemId, Sprite)> addMenu = database.ConvertAll(new Converter<Item, (ItemId itemId, Sprite sprite)>(ItemToItemIdSprite));
            OnDatabaseBuilt?.Invoke(addMenu);
        }

        public Item generateById(ItemId id)
        {
            return database.Find(item => item.id == id);
        }

        /// <summary>
        /// Auxiliar method to map an <see cref="Item"/> to (<see cref="ItemId"/>, <see cref="Sprite"/>)
        /// </summary>
        /// <param name="item">Item to be mapped</param>
        /// <returns></returns>
        private (ItemId, Sprite) ItemToItemIdSprite(Item item)
        {
            return (item.id, item.texture);
        }
    }
}

