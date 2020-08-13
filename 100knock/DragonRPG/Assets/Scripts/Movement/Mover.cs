using UnityEngine;
using UnityEngine.AI;

namespace RPG.Core
{
    public class Mover : MonoBehaviour
    {
        private void Update()
        {
            this.UpdateAnimation();
        }

        public void MoveTo(Vector3 destination)
        {
            var navMeshAgent = this.GetComponent<NavMeshAgent>();
            Debug.Assert(navMeshAgent != null);

            navMeshAgent.destination = destination;
        }

        private void UpdateAnimation()
        {
            var velocity = this.GetComponent<NavMeshAgent>().velocity;
            // ローカル座標系に変換してローカルオブジェクトを中心にした速度ベクトルに変える
            // オブジェクトを中心した移動速度なのでワールド座標系だと具合が悪い
            // 回転移動時の移動速度が正確でなくなるから
            var localVelocity = this.transform.InverseTransformDirection(velocity);

            var speed = localVelocity.z;
            this.GetComponent<Animator>().SetFloat("fowardSpeed", speed);
        }
    }
}
