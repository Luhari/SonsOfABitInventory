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

    public class RestoreLifeAction : IAction
    {
        public event Action<string> OnActionPerformed;
        private float amountHealed;
        public RestoreLifeAction(float amountHealed)
        {
            this.amountHealed = amountHealed;
        }

        public void PerformAction()
        {
            OnActionPerformed?.Invoke($"Healed {amountHealed} HP");
        }
    }

    public class RestoreManaAction : IAction
    {
        public event Action<string> OnActionPerformed;
        private float amountRecovered;
        public RestoreManaAction(float amountRecovered)
        {
            this.amountRecovered = amountRecovered;
        }

        public void PerformAction()
        {
            OnActionPerformed?.Invoke($"Healed {amountRecovered} HP");
        }
    }

}
