using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float speed = 1f;
        [SerializeField]
        private bool isHoming = false;
        [SerializeField]
        private GameObject hitEffect = null;
        [SerializeField]
        private GameObject[] immediateGameObjects = new GameObject[0];
        [SerializeField]
        private float delayDestryTimeSecond = 10f;

        private Health target = null;
        private float damage = 0f;
        private GameObject instigator = null;

        public void SetTarget(Health target, float damage, GameObject instigator)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
        }

        private void Start()
        {
            this.transform.LookAt(this.GetAimLocation());
        }

        private void Update()
        {
            if (this.target == null) return;
            if (this.isHoming && !this.target.IsDead())
            {
                this.transform.LookAt(this.GetAimLocation());
            }
            // 変更された座標軸に対してZ方向にまっすぐ移動するようにする
            this.transform.Translate(Vector3.forward * this.speed * Time.deltaTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider capsule = this.target.GetComponent<CapsuleCollider>();
            if (capsule == null) return this.target.transform.position;

            Vector3 worldPosition = this.target.transform.position + capsule.height * Vector3.up / 2;
            return worldPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (this.target == null) return;
            if (this.target.IsDead()) return;
            if (other.GetComponent<Health>() != this.target) return;

            this.target.TakeDamge(this.instigator, this.damage);

            this.speed = 0f;
            for (int i = 0; i < this.immediateGameObjects.Length; ++i)
            {
                GameObject.Destroy(this.immediateGameObjects[i]);
            }

            GameObject.Destroy(this.gameObject, this.delayDestryTimeSecond);

            if (this.hitEffect != null)
            {
                GameObject.Instantiate(this.hitEffect, this.GetAimLocation(), this.transform.rotation);
            }
        }
    }
}