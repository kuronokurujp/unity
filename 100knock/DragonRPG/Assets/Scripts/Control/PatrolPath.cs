using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private const float wayPointGizmoSphereRadius = 0.5f;

        /// <summary>
        /// Callback to draw gizmos that are pickable and always drawn.
        /// </summary>
        private void OnDrawGizmos()
        {
            for (int i = 0; i < this.transform.childCount; ++i)
            {
                Gizmos.DrawSphere(this.GetChildPosition(i), wayPointGizmoSphereRadius);
                Gizmos.DrawLine(this.GetChildPosition(i), this.GetChildPosition(this.GetNextIndex(i)));
            }
        }
        public int GetNextIndex(int i)
        {
            int nextIndex = i + 1;
            if (this.transform.childCount <= nextIndex)
            {
                return 0;
            }

            return nextIndex;
        }
        public Vector3 GetChildPosition(int i)
        {
            return this.transform.GetChild(i).position;
        }
    }   
}

