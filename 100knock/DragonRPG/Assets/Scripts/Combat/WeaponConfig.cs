﻿using System;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon Menu/Weapon New Data", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField]
        private Weapon equippedWeaponPrefab = null;
        [SerializeField]
        private AnimatorOverrideController animatorOverrideCtrl = null;
        [SerializeField]
        private float weaponRange = 2.0f;
        [SerializeField]
        private float weaponDamagePercentageBonus = 0f;
        [SerializeField]
        private float weaponDamage = 5.0f;
        [SerializeField]
        private bool isRightHand = true;
        [SerializeField]
        private Projectile projecttilePrefab = null;
        private const string weaponObjectName = "weapon";

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            this.DestoryOldWeapon(rightHand, leftHand);

            Weapon nowWeapon = null;
            if (this.equippedWeaponPrefab != null)
            {
                Transform handTransform = GetHandTransform(rightHand, leftHand);
                nowWeapon = GameObject.Instantiate(this.equippedWeaponPrefab, handTransform, false);
                nowWeapon.gameObject.name = weaponObjectName;
            }

            if (animator == null)
            {
                return null;
            }

            var overrideAnim = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (this.animatorOverrideCtrl != null)
            {
                animator.runtimeAnimatorController = this.animatorOverrideCtrl;
            }
            // 設定するアニメータがない場合はすでに設定しているのを再設定する
            else if (overrideAnim != null)
            {
                animator.runtimeAnimatorController = overrideAnim;
            }

            return nowWeapon;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float baseDamage)
        {
            Debug.Assert(this.projecttilePrefab != null);
            var handTransform = this.GetHandTransform(rightHand, leftHand);
            Projectile projectile = GameObject.Instantiate(this.projecttilePrefab, handTransform.position, Quaternion.identity);
            projectile.SetTarget(target, /*this.weaponDamage*/baseDamage, instigator);
        }

        public bool HaveProjectile()
        {
            return this.projecttilePrefab != null;
        }

        public float GetDamage() { return this.weaponDamage; }
        public float GetPercentageBonus() { return this.weaponDamagePercentageBonus; }
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
