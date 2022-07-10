using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class Player : MonoBehaviour
    {
        private Inventory inventory;

        private float m_hp = 100;
        private float m_mana = 100;

        // Start is called before the first frame update
        void Start()
        {
            inventory = gameObject.GetComponent<Inventory>();

            inventory.OnActionPerformed += HandleOnActionPerfomed;
        }

        // Instead of string, maybe pass string for an action and value of the action
        private void HandleOnActionPerfomed(string data)
        {
            Debug.Log(data);
        }

    }
}
