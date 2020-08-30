using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject presistentObjectPrefab = null;
        private static bool hasSpawned = false;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (hasSpawned) return;

            this.SpawnPersistentObject();

            hasSpawned = true;
        }
        private void SpawnPersistentObject()
        {
            GameObject persistentObject = GameObject.Instantiate(this.presistentObjectPrefab);
            GameObject.DontDestroyOnLoad(persistentObject);
        }
    }
}

