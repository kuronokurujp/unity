﻿using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField]
        private float moveSpeed = 6f;

        private NavMeshAgent navMeshAgent = null;
        private Health health = null;

        private void Start()
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
            // オブジェクトを中心した移動速度なのでワールド座標系だと具合が悪い
            // 回転移動時の移動速度が正確でなくなるから
            var localVelocity = this.transform.InverseTransformDirection(velocity);

            var speed = localVelocity.z;
            this.GetComponent<Animator>().SetFloat("fowardSpeed", speed);
        }
    }
}
