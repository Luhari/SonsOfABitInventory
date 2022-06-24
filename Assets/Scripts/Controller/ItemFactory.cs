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
                new Weapon(
                    id: ItemId.SWORD, name : "Sword", weight: 10f, marketValue: 1f, texture: "sword",
                    dps: 1f),
                new Weapon(id: ItemId.BOW, name: "Bow", weight: 1f, marketValue: 1f, texture: "bow",
                    dps: 1f),
                new ConsumableItem(id: ItemId.HEALTH_POTION, name: "Health Potion", weight: 3f, texture: "healthPotion0",
                    timeBetweenDeteriorationLevel: 1f, 
                    maxDeteriorationLevel: 3, 
                    action: new UseHealthPotion(10f)),
                new ConsumableItem(id: ItemId.MANA_POTION, name: "Mana Potion", weight: 1f, texture: "manaPotion0",
                    timeBetweenDeteriorationLevel: 3f,
                    maxDeteriorationLevel: 3, 
                    action: new UseManaPotion(1f)),
                new ResourceItem(id: ItemId.ARROW, name: "Arrow", weight: 1f, marketValue: 10f, texture: "arrow0", 
                    timeBetweenDeteriorLevel: 60f, 
                    marketValueLostAtDeterioring: 1f, 
                    maxDeteriorationLevel: 3),
                new ResourceItem(id: ItemId.WOOD, name: "Wood", weight: 1f, marketValue: 10f, texture: "wood0", 
                    timeBetweenDeteriorLevel: 3f, 
                    marketValueLostAtDeterioring: 1f, 
                    maxDeteriorationLevel: 3),
                new TrashItem(id: ItemId.TRASH, name: "Trash", weight: 1f),
            };

            List<(ItemId, Sprite)> addMenu = database.ConvertAll(new Converter<Item, (ItemId itemId, Sprite sprite)>(ItemToItemIdSprite));
            OnDatabaseBuilt?.Invoke(addMenu);
        }

        public Item generateById(ItemId id)
        {
            Item databaseItem =  database.Find(item => item.id == id);

            return databaseItem.Clone();
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

