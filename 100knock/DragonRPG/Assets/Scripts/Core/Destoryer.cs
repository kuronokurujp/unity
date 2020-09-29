using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Destoryer : MonoBehaviour
    {
        [SerializeField]
        private GameObject destoryTarget = null;

        public void DetoryToTaget()
        {
            GameObject.Destroy(this.destoryTarget);
        }
    }
}

