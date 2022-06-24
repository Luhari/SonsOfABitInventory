using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory
{
    public class DeteriorationTimerController : MonoBehaviour
    {
        public event Action<DeteriorableItem> OnDeterioration;

        private Dictionary<DeteriorableItem, float> trackingTimers = new Dictionary<DeteriorableItem, float>();

        void Update()
        {
            foreach (KeyValuePair<DeteriorableItem, float> timer in trackingTimers.ToArray())
            {
                trackingTimers[timer.Key] -= Time.deltaTime;

                if (trackingTimers[timer.Key] < 0)
                {
                    trackingTimers.Remove(timer.Key);
                    OnDeterioration?.Invoke(timer.Key);
                }
            }
        }

        public void AddToTrack(DeteriorableItem item)
        {
            if (trackingTimers.ContainsKey(item))
            {
                trackingTimers[item] = item.timeBetweenDeteriorationLevel;
            } 
            else
            {
                trackingTimers.Add(item, item.timeBetweenDeteriorationLevel);
            }
        }
    }
}
