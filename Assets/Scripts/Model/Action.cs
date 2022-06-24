using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    public interface IAction
    {
        event Action<string> OnActionPerformed;
        void PerformAction();
    }

    public class UseHealthPotion : IAction
    {
        public event Action<string> OnActionPerformed;
        private float amountHealed;
        public UseHealthPotion(float amountHealed)
        {
            this.amountHealed = amountHealed;
        }

        public void PerformAction()
        {
            Debug.Log($"Healed {amountHealed} HP");
        }
    }

    public class UseManaPotion : IAction
    {
        public event Action<string> OnActionPerformed;
        private float amountRecovered;
        public UseManaPotion(float amountRecovered)
        {
            this.amountRecovered = amountRecovered;
        }

        public void PerformAction()
        {
            Debug.Log($"Recovered {amountRecovered} MP");
        }
    }

}
