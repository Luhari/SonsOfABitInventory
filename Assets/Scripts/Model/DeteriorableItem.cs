using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{ 
    /// <summary>
    /// Subclass of <see cref="Item"/> that that has deterioration with time and once deteriorated, 
    /// loses certain market value
    /// </summary>
    public class DeteriorableItem : Item
    {
        /// <summary>
        /// Current deterioration level
        /// </summary>
        public int m_deteriorationLevel { get; protected set; }
        /// <summary>
        /// Time in seconds between levels of deterioriation
        /// </summary>
        public float m_timeBetweenDeteriorationLevel { get; protected set; }

        public int m_maxDeteriorationLevel { get; protected set; }

        public event Action<DeteriorableItem> OnDeterioration;

        /// <summary>
        /// Constructor of DeteriorableItem
        /// </summary>
        /// <param name="id">Id of the item form database</param>
        /// <param name="name">Name of the item</param>
        /// <param name="weight">Weight of the item</param>
        /// <param name="timeBetweenDeteriorationLevel">Time in seconds between deterioration levels</param>
        /// <param name="maxDeteriorationLevel">Max deterioration level</param>
        public DeteriorableItem(ItemId id, string name, float weight, float timeBetweenDeteriorationLevel, int maxDeteriorationLevel, float marketValue = 0)
        {
            m_id = id;
            m_name = name;
            m_weight = weight;
            m_deteriorationLevel = 0;
            m_timeBetweenDeteriorationLevel = timeBetweenDeteriorationLevel;
            m_maxDeteriorationLevel = maxDeteriorationLevel;
            m_marketValue = marketValue;
        }

        /// <summary>
        /// Forces one deterioration level and updates the texture of the item and returns true
        /// if the item can deterior another level
        /// </summary>
        public bool DeteriorOneLevel()
        {
            if (++m_deteriorationLevel <= m_maxDeteriorationLevel)
            {
                Sprite currentTexture = m_texture;

                m_texture = Resources.Load<Sprite>("Sprites/Items/" + currentTexture.name.Substring(0, currentTexture.name.Length -1) + m_deteriorationLevel);

                // OnDeterioration?.Invoke(this);
            }
            return m_deteriorationLevel < m_maxDeteriorationLevel;
        }

        public override Item Clone()
        {
            throw new NotImplementedException();
        }
    }
}
