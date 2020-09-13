using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon Menu/Weapon New Data", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField]
        private GameObject equippedWeaponPrefab = null;
        [SerializeField]
        private AnimatorOverrideController animatorOverrideCtrl = null;
        [SerializeField]
        private float weaponRange = 2.0f;
        [SerializeField]
        private float weaponDamage = 5.0f;
        [SerializeField]
        private bool isRightHand = true;
        [SerializeField]
        private Projectile projecttilePrefab = null;

        private const string weaponObjectName = "weapon";

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            this.DestoryOldWeapon(rightHand, leftHand);

            if (this.equippedWeaponPrefab != null)
            {
                Transform handTransform = GetHandTransform(rightHand, leftHand);
                var newSowrdWeaponObject = GameObject.Instantiate(this.equippedWeaponPrefab, handTransform, false);
                newSowrdWeaponObject.name = weaponObjectName;
            }

            if (this.animatorOverrideCtrl != null && animator != null)
            {
                animator.runtimeAnimatorController = this.animatorOverrideCtrl;
            }
        }
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Debug.Assert(this.projecttilePrefab != null);
            var handTransform = this.GetHandTransform(rightHand, leftHand);
            Projectile projectile = GameObject.Instantiate(this.projecttilePrefab, handTransform.position, Quaternion.identity);
            projectile.SetTarget(target, this.weaponDamage);
        }
        public bool HaveProjectile()
        {
            return this.projecttilePrefab != null;
        }
        public float GetDamage() { return this.weaponDamage; }
        public float GetRange() {return this.weaponRange; }
        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform = leftHand;
            if (isRightHand) handTransform = rightHand;

            Debug.Assert(handTransform != null);
            return handTransform;
        }
        private void DestoryOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform destroyWeaponTranform = rightHand.transform.Find(weaponObjectName);
            if (destroyWeaponTranform == null)
            {
                destroyWeaponTranform = leftHand.transform.Find(weaponObjectName);
            }

            if (destroyWeaponTranform == null) return;

            // 破棄オブジェクトの名前を変える事でFindで取得出来ないようにする
            destroyWeaponTranform.name = "DESTORYING";
            GameObject.Destroy(destroyWeaponTranform.gameObject);
            destroyWeaponTranform = null;
        }
    }
}
