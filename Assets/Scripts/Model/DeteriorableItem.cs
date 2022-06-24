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
        public override ItemId id { get; protected set; }
        public override string name { get; protected set; }
        public override float marketValue { get; protected set; }
        public override float weight { get; protected set; }
        public override Sprite texture { get; protected set; }

        /// <summary>
        /// Current deterioration level
        /// </summary>
        public int deteriorationLevel { get; protected set; }
        /// <summary>
        /// Time in seconds between levels of deterioriation
        /// </summary>
        public float timeBetweenDeteriorationLevel { get; protected set; }

        public int maxDeteriorationLevel { get; protected set; }

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
            this.id = id;
            this.name = name;
            this.weight = weight;
            deteriorationLevel = 0;
            this.timeBetweenDeteriorationLevel = timeBetweenDeteriorationLevel;
            this.maxDeteriorationLevel = maxDeteriorationLevel;
            this.marketValue = marketValue;
        }

        /// <summary>
        /// Forces one deterioration level and updates the texture of the item and returns true
        /// if the item can deterior another level
        /// </summary>
        public bool deteriorOneLevel()
        {
            if (++deteriorationLevel <= maxDeteriorationLevel)
            {
                Sprite currentTexture = this.texture;

                this.texture = Resources.Load<Sprite>("Sprites/Items/" + currentTexture.name.Substring(0, currentTexture.name.Length -1) + deteriorationLevel);

                // OnDeterioration?.Invoke(this);
            }
            return deteriorationLevel < maxDeteriorationLevel;
        }

        public override Item Clone()
        {
            throw new NotImplementedException();
        }
    }
}
