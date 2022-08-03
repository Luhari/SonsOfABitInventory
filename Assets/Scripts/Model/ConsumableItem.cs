using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    /// <summary>
    /// Subclass of <see cref="DeteriorableItem"/> that can be used, can't be sold, has no value and when it completely deteriorates,
    /// it transforms into <see cref="TrashItem"/>
    /// </summary>
    public class ConsumableItem : DeteriorableItem, IConsumable
    {
        private IAction m_action;

        public IAction action { get => m_action; }

        /// <summary>
        /// Constructor of ConsumableItem
        /// </summary>
        /// <param name="id">Id of the item from database</param>
        /// <param name="name">Name of the item</param>
        /// <param name="weight">Weight of the item</param>
        /// <param name="texture">Name of the png to use as sprite</param>
        /// <param name="timeBetweenDeteriorationLevel">Time in seconds between deterioration levels</param>
        public ConsumableItem(ItemId id, string name, float weight, string texture, 
            float timeBetweenDeteriorationLevel, int maxDeteriorationLevel, IAction action) : 
            base(id, name, weight, timeBetweenDeteriorationLevel, maxDeteriorationLevel)
        {
            m_marketValue = 0;
            m_texture = Resources.Load<Sprite>("Sprites/Items/" + texture);
            m_action = action;
        }

        public ConsumableItem(ConsumableItem item) : 
            base(item.m_id, item.m_name, item.m_weight, item.m_timeBetweenDeteriorationLevel, item.m_maxDeteriorationLevel)
        {
            m_texture = item.m_texture;
            m_marketValue = item.m_marketValue;
            m_action = item.m_action;
        }

        public override Item Clone()
        {
            return new ConsumableItem(this);
        }

        public void PerformAction()
        {
            m_action?.PerformAction();
        }
    }

    interface IConsumable
    {
        public IAction action { get; }

        public void PerformAction();
    }
}
