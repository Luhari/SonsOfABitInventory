using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    /// <summary>
    /// Subclass of Item that can be used to attack and can require a <see cref="ResourceItem"/> to be used
    /// </summary>
    public class Weapon : Item
    {
        public override ItemId id { get; protected set; }
        public override string name { get; protected set; }
        public override float marketValue { get; protected set; }
        public override float weight { get; protected set; }
        public override Sprite texture { get; protected set; }

        public float dps { get; private set; }

        /// <summary>
        /// Constructor of a weapon
        /// </summary>
        /// <param name="id">Id of the item from the database</param>
        /// <param name="name">Name of the item</param>
        /// <param name="weight">Weight of the item</param>
        /// <param name="dps">Damage per second</param>
        /// <param name="texture">Name of the png to use as sprite</param>
        /// <param name="marketValue">Market value the weapon</param>
        public Weapon(ItemId id, string name, float weight, float dps, string texture, float marketValue) 
        {
            this.id = id;
            this.name = name;
            this.weight = weight;
            this.dps = dps;
            this.marketValue = marketValue;
            this.texture = Resources.Load<Sprite>("Sprites/Items/" + texture);
        }

        public Weapon(Weapon item)
        {
            this.id = item.id;
            this.name = item.name;
            this.weight = item.weight;
            this.dps = item.dps;
            this.marketValue = item.marketValue;
            this.texture = item.texture;
        }

        public override Item Clone()
        {
            return new Weapon(this);
        }
    }
}