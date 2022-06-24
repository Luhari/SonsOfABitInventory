using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{

    /// <summary>
    /// Subclass of <see cref="Item"/> that doesn't have market value and cant' be used
    /// </summary>
    public class TrashItem : Item
    {
        public override ItemId id { get; protected set; }
        public override string name { get; protected set; }
        public override float marketValue { get; protected set; }
        public override float weight { get; protected set; }
        public override Sprite texture { get; protected set; }

        /// <summary>
        /// Constructor of a TrashItem
        /// </summary>
        /// <param name="id">Id of the item from the database</param>
        /// <param name="name">Name of the item</param>
        /// <param name="weight">Weight of the item</param>
        public TrashItem(ItemId id, string name, float weight)
        {
            this.id = id;
            this.name = name;
            this.weight = weight;
            marketValue = 0;
            texture = Resources.Load<Sprite>("Sprites/Items/trash");
        }

        public TrashItem(TrashItem item) 
        {
            this.id = item.id;
            this.name = item.name;
            this.weight = item.weight;
            this.marketValue = item.marketValue;
            this.texture = item.texture;
        }

        public override Item Clone()
        {
            return new TrashItem(this);
        }

        public void SetWeight(float weight)
        {
            this.weight = weight;
        }
    }
}
