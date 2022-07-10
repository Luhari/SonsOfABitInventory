using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    /// <summary>
    /// Subclass of <see cref="DeteriorableItem"/> that has market value, deterioration with time and once deteriorated, 
    /// loses market value
    /// </summary>
    public class ResourceItem : DeteriorableItem
    {
        /// <summary>
        /// Market value that will lose the resource after reaching a deterioration level
        /// </summary>
        public float m_marketValueLostAtDeterioring { get; private set; }

        /// <summary>
        /// Constructor of ResourceItem 
        /// </summary>
        /// <param name="id">Id of the item from the database</param>
        /// <param name="name">Name of the item</param>
        /// <param name="weight">Weight of the item</param>
        /// <param name="texture">Name of the png to use as sprite</param>
        /// <param name="timeBetweenDeteriorLevel">Time in seconds between levels of deterioration</param>
        /// <param name="marketValueLostAtDeterioring">Market value to lose after reaching another level of deterioration</param>
        public ResourceItem(ItemId id, string name, float weight, string texture, float timeBetweenDeteriorLevel, float marketValueLostAtDeterioring, int maxDeteriorationLevel, float marketValue) 
            : base(id, name, weight, timeBetweenDeteriorLevel, maxDeteriorationLevel, marketValue)
        {
            m_marketValueLostAtDeterioring = marketValueLostAtDeterioring;
            m_texture = Resources.Load<Sprite>("Sprites/Items/" + texture);
        }

        public ResourceItem(ResourceItem item) 
            : base(item.m_id, item.m_name, item.m_weight, item.m_timeBetweenDeteriorationLevel, item.m_maxDeteriorationLevel, item.m_marketValue)
        {
            m_marketValueLostAtDeterioring = item.m_marketValueLostAtDeterioring;
            m_texture = item.m_texture;
        }

        public override Item Clone()
        {
            return new ResourceItem(this);
        }

        /// <summary>
        /// Substract <see cref="marketValueLostAtDeterioring"/> from <see cref="Item.marketValue"/>. 
        /// The min value of <see cref="Item.marketValue"/> is 0
        /// </summary>
        public void LoseMarketValueAtDeterioring()
        {
            m_marketValue -= m_marketValueLostAtDeterioring;
            if (m_marketValue < 0) m_marketValue = 0;
        }
    }
}
