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
        public float marketValueLostAtDeterioring;

        /// <summary>
        /// Constructor of ResourceItem 
        /// </summary>
        /// <param name="id">Id of the item from the database</param>
        /// <param name="name">Name of the item</param>
        /// <param name="weight">Weight of the item</param>
        /// <param name="texture">Name of the png to use as sprite</param>
        /// <param name="timeBetweenDeteriorLevel">Time in seconds between levels of deterioration</param>
        /// <param name="marketValueLostAtDeterioring">Market value to lose after reaching another level of deterioration</param>
        public ResourceItem(ItemId id, string name, float weight, string texture, float timeBetweenDeteriorLevel, float marketValueLostAtDeterioring, int maxDeteriorationLevel) : base(id, name, weight, timeBetweenDeteriorLevel, maxDeteriorationLevel)
        {
            this.marketValueLostAtDeterioring = marketValueLostAtDeterioring;
            this.texture = Resources.Load<Sprite>("Sprites/Items/" + texture);
        }

        /// <summary>
        /// Substract <see cref="marketValueLostAtDeterioring"/> from <see cref="Item.marketValue"/>. 
        /// The min value of <see cref="Item.marketValue"/> is 0
        /// </summary>
        public void loseMarketValueAtDeterioring()
        {
            marketValue -= marketValueLostAtDeterioring;
            if (marketValue < 0) marketValue = 0;
        }
    }
}
