using System.Collections;
using System.Collections.Generic;
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

        public void Spawn(Transform handTransform, Animator animator)
        {
            if (this.equippedWeaponPrefab != null && handTransform != null) 
            {
                var newSowrdWeaponObject = GameObject.Instantiate(this.equippedWeaponPrefab, handTransform);
                newSowrdWeaponObject.transform.localPosition = Vector3.zero;
                newSowrdWeaponObject.transform.localRotation = Quaternion.identity;
            }

            if (this.animatorOverrideCtrl != null && animator != null)
            {
                animator.runtimeAnimatorController = this.animatorOverrideCtrl;
            }
        }

        public float GetDamage() { return this.weaponDamage; }
        public float GetRange() {return this.weaponRange; }
    }
}
