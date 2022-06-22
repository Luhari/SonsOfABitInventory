using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Subclass of <see cref="DeteriorableItem"/> that can be used, can't be sold, has no value and when it completely deteriorates,
/// it transforms into <see cref="TrashItem"/>
/// </summary>
public class ConsumableItem : DeteriorableItem
{
    /// <summary>
    /// Constructor of ConsumableItem
    /// </summary>
    /// <param name="id">Id of the item from database</param>
    /// <param name="name">Name of the item</param>
    /// <param name="weight">Weight of the item</param>
    /// <param name="texture">Name of the png to use as sprite</param>
    /// <param name="timeBetweenDeteriorationLevel">Time in seconds between deterioration levels</param>
    public ConsumableItem(ItemId id, string name, float weight, string texture, float timeBetweenDeteriorationLevel, int maxDeteriorationLevel): base(id, name, weight, timeBetweenDeteriorationLevel, maxDeteriorationLevel)
    {
        marketValue = 0;
        this.texture = Resources.Load<Sprite>("Sprites/Items/" + texture);
    }

    /// <summary>
    /// 
    /// </summary>
    public void Use()
    {

    }
}
