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
        public float m_dps { get; private set; }

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
            m_id = id;
            m_name = name;
            m_weight = weight;
            m_dps = dps;
            m_marketValue = marketValue;
            m_texture = Resources.Load<Sprite>("Sprites/Items/" + texture);
        }

        public Weapon(Weapon item)
        {
            m_id = item.m_id;
            m_name = item.m_name;
            m_weight = item.m_weight;
            m_dps = item.m_dps;
            m_marketValue = item.m_marketValue;
            m_texture = item.m_texture;
        }

        public override Item Clone()
        {
            return new Weapon(this);
        }
    }
}