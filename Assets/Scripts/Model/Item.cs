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
        public ItemId m_id { get; protected set; }
        public string m_name { get; protected set; }
        public float m_weight { get; protected set; }
        public float m_marketValue { get; protected set; }
        public Sprite m_texture { get; protected set; }

        public abstract Item Clone();
    }
}
