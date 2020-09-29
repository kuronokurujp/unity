using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class DestoryAfterEffect : MonoBehaviour
    {
        [SerializeField]
        private GameObject destoryTarget = null;

        private void Update()
        {
            if (!this.GetComponent<ParticleSystem>().IsAlive())
            {
                if (this.destoryTarget != null)
                {
                    GameObject.Destroy(this.destoryTarget);
                }
                else
                {
                    GameObject.Destroy(this.gameObject);
                }
            }
        }
    }
}

