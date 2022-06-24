using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    /// <summary>
    /// Abstract Class of an item that has an id, name, weight as default. Its subclasses can define
    /// its marketValue and its texture
    /// </summary>
    public abstract class Item
    {
        public abstract ItemId id { get; protected set; }
        public abstract string name { get; protected set; }
        public abstract float weight { get; protected set; }
        public abstract float marketValue { get; protected set; }
        public abstract Sprite texture { get; protected set; }

        public abstract Item Clone();
    }
}
