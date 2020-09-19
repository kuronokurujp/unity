using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField]
        private Weapon weaponData = null;
        [SerializeField]
        private float pickupShowSecond = 5f;

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<Fighter>().EquipWeapon(this.weaponData);

                this.StartCoroutine(this.RespwanPickupEvent(this.pickupShowSecond));
            }
        }

        private IEnumerator RespwanPickupEvent(float showSecond)
        {
            this.ShowPickup(false);
            yield return new WaitForSeconds(showSecond);
            this.ShowPickup(true);
        }

        private void ShowPickup(bool show)
        {
            this.GetComponent<Collider>().enabled = show;
            for (int i = 0; i < this.transform.childCount; ++i)
            {
                this.transform.GetChild(i).transform.gameObject.SetActive(show);
            }
        }
    }
}

