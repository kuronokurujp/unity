using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target = null;

        private void LateUpdate()
        {
            Debug.Assert(this.target != null);
            this.transform.position = this.target.position;
        }
    }
}
