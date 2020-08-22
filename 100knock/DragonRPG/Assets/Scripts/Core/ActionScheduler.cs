using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        private IAction currentAction = null;

        public void StartAction(IAction action)
        {
            if (this.currentAction == action) return;
            if (this.currentAction != null)
            {
                this.currentAction.Cancel();
            }

            this.currentAction = action;
        }

        public void CancelCurrentAction()
        {
            this.StartAction(null);
        }
    }
}

