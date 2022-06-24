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
        public override string textureName { get; protected set; }

        /// <summary>
        /// Current deterioration level
        /// </summary>
        public float deteriorationLevel { get; protected set; }
        /// <summary>
        /// Time in seconds between levels of deterioriation
        /// </summary>
        public float timeBetweenDeteriorationLevel { get; protected set; }
        /// <summary>
        /// Time in seconds left before the next deterioration level
        /// </summary>
        private float timeLeftBeforeNextDeteriorationLevel;
        private int maxDeteriorationLevel;

        /// <summary>
        /// Constructor of DeteriorableItem
        /// </summary>
        /// <param name="id">Id of the item form database</param>
        /// <param name="name">Name of the item</param>
        /// <param name="weight">Weight of the item</param>
        /// <param name="timeBetweenDeteriorationLevel">Time in seconds between deterioration levels</param>
        /// <param name="maxDeteriorationLevel">Max deterioration level</param>
        public DeteriorableItem(ItemId id, string name, float weight, float timeBetweenDeteriorationLevel, int maxDeteriorationLevel)
        {
            this.id = id;
            this.name = name;
            this.weight = weight;
            deteriorationLevel = 0;
            this.timeBetweenDeteriorationLevel = timeBetweenDeteriorationLevel;
            timeLeftBeforeNextDeteriorationLevel = timeBetweenDeteriorationLevel;
            this.maxDeteriorationLevel = maxDeteriorationLevel;
        }

        /// <summary>
        /// 
        /// </summary>
        public void forceOneDeteriorLevel()
        {
            if (++deteriorationLevel <= maxDeteriorationLevel)
            {
                timeLeftBeforeNextDeteriorationLevel = timeBetweenDeteriorationLevel;
                // Here restart timer
            }
        }

        // add timer to manage deterioration level
    }
}
