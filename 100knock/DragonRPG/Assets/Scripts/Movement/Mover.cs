using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable 
    {
        [SerializeField]
        private float moveSpeed = 6f;
        [SerializeField]
        private float navMeshPathLength = 40f;

        private NavMeshAgent navMeshAgent = null;
        private Health health = null;

        private void Awake()
        {
            this.navMeshAgent = this.GetComponent<NavMeshAgent>();
            Debug.Assert(this.navMeshAgent != null);

            this.health = this.GetComponent<Health>();
        }

        private void Update()
        {
            // 死んだ場合Navemeshを無効にする事で死んだキャラを障害物にしないようにする
            this.navMeshAgent.enabled = !this.health.IsDead();
            this.UpdateAnimation();
        }

        public bool CanMoveTo(Vector3 destantion)
        {
            // 移動距離を取得して移動可能距離かどうかチェック
            if (this.GetNavMeshPathLength(destantion) > this.navMeshPathLength) return false;

            return true;
        }

        private float GetNavMeshPathLength(Vector3 targetPosition)
        {
            float totalDistance = 0f;
            NavMeshPath navMeshPath = new NavMeshPath();
            if (NavMesh.CalculatePath(this.transform.position, targetPosition, NavMesh.AllAreas, navMeshPath))
                return totalDistance;

            if (navMeshPath.corners.Length < 2)
                return totalDistance;

            for (int i = 0; i < navMeshPath.corners.Length - 1; i++)
            {
                totalDistance += Vector3.Distance(navMeshPath.corners[i], navMeshPath.corners[i + 1]);
            }

            return totalDistance;
        }


        public void StartMoveAction(Vector3 destination, float moveSpeedFraction)
        {
            this.GetComponent<ActionScheduler>().StartAction(this);
            this.MoveTo(destination, moveSpeedFraction);
        }
        public void MoveTo(Vector3 destination, float moveSpeedFraction)
        {
            this.navMeshAgent.destination = destination;
            this.navMeshAgent.speed = this.moveSpeed * Mathf.Clamp01(moveSpeedFraction);
            this.navMeshAgent.isStopped = false;
        }
        public void Cancel()
        {
            this.navMeshAgent.isStopped = true;
        }
        private void UpdateAnimation()
        {
            var velocity = this.GetComponent<NavMeshAgent>().velocity;
            // ローカル座標系に変換してローカルオブジェクトを中心にした速度ベクトルに変える
            // オブジェクトを中心にした移動速度なのでワールド座標系だと具合が悪い
            // 回転移動時の移動速度が正確でなくなるから
            var localVelocity = this.transform.InverseTransformDirection(velocity);

            var speed = localVelocity.z;
            this.GetComponent<Animator>().SetFloat("fowardSpeed", speed);
        }
        public object CaptureState()
        {
            var saveData = new SaveData();
            saveData.position = new SerializableVector3(this.transform.position);

            return saveData;
        }
        public void RestoreState(object state)
        {
            var saveData = (SaveData)state;
            this.GetComponent<NavMeshAgent>().enabled = false;

            this.transform.position = saveData.position.ToVector();

            this.GetComponent<NavMeshAgent>().enabled = true;
        }

        [System.Serializable]
        private struct SaveData
        {
            public SerializableVector3 position;
        }
    }
}
