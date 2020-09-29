using System.Collections;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField]
        private WeaponConfig weaponData = null;
        [SerializeField]
        private float pickupShowSecond = 5f;
        [SerializeField]
        private float heal = 0f;

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        public bool HandleRaycast(PlayerController player)
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.Pickup(player.gameObject);
            }

            return true;
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                this.Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject)
        {
            if (this.weaponData != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(this.weaponData);
            }

            if (this.heal > 0f)
            {
                subject.GetComponent<Health>().Heal(this.heal);
            }

            this.StartCoroutine(this.RespwanPickupEvent(this.pickupShowSecond));
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

