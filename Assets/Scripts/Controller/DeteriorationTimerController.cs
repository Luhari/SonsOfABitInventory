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
        private Dictionary<DeteriorableItem, float> m_trackingTimers = new Dictionary<DeteriorableItem, float>();

        public event Action<DeteriorableItem> OnDeterioration;

        void Update()
        {
            foreach (KeyValuePair<DeteriorableItem, float> timer in m_trackingTimers.ToArray())
            {
                if (m_trackingTimers.ContainsKey(timer.Key))
                {
                    m_trackingTimers[timer.Key] -= Time.deltaTime;

                    if (m_trackingTimers[timer.Key] < 0)
                    {
                        m_trackingTimers.Remove(timer.Key);
                        OnDeterioration?.Invoke(timer.Key);
                    }

                }
            }
        }

        public void AddToTrack(DeteriorableItem item)
        {
            if (m_trackingTimers.ContainsKey(item))
            {
                m_trackingTimers[item] = item.m_timeBetweenDeteriorationLevel;
            } 
            else
            {
                m_trackingTimers.Add(item, item.m_timeBetweenDeteriorationLevel);
            }
        }

        public void StopTracking(DeteriorableItem item)
        {
            if (m_trackingTimers.ContainsKey(item))
            {
                m_trackingTimers.Remove(item);
            }
        }
    }
}
