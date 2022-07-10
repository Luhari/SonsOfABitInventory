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
        /// <summary>
        /// Constructor of a TrashItem
        /// </summary>
        /// <param name="id">Id of the item from the database</param>
        /// <param name="name">Name of the item</param>
        /// <param name="weight">Weight of the item</param>
        public TrashItem(ItemId id, string name, float weight)
        {
            m_id = id;
            m_name = name;
            m_weight = weight;
            m_marketValue = 0;
            m_texture = Resources.Load<Sprite>("Sprites/Items/trash");
        }

        public TrashItem(TrashItem item) 
        {
            m_id = item.m_id;
            m_name = item.m_name;
            m_weight = item.m_weight;
            m_marketValue = item.m_marketValue;
            m_texture = item.m_texture;
        }

        public override Item Clone()
        {
            return new TrashItem(this);
        }

        public void SetWeight(float weight)
        {
            m_weight = weight;
        }
    }
}
