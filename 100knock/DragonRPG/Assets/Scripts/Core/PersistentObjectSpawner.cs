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
        /// Awakeでは基本ルールとしてメソッドを呼ばないが、
        /// ここでは例外
        /// なぜならゲームシステム構築をここで最初に行う必要があるので
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

