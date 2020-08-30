using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        /// <summary>
        /// 入り口と出口の接続先
        /// </summary>
        private enum DestinationIdentify
        {
            A, B, C, D,
        }

        [SerializeField]
        private int loadSceneNumber = -1;
        [SerializeField]
        private GameObject spwan = null;
        [SerializeField]
        private DestinationIdentify destination = DestinationIdentify.A;
        [SerializeField]
        private float fadeOutTime = 1f;
        [SerializeField]
        private float fadeWaitTime = 1f;
        [SerializeField]
        private float fadeInTime = 1f;

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            this.StartCoroutine(this.Transition());
        }
        private IEnumerator Transition()
        {
            if (this.loadSceneNumber < 0)
            {
                Debug.LogError("scene load number is not set");
                yield break;
            }

            // シーン移動した事でobjectが破棄されてしまい、ロード以降の処理をしなくなるのを防ぐため
            // 破棄不可能にオブジェクトにする
            GameObject.DontDestroyOnLoad(this.gameObject);

            var fader = GameObject.FindObjectOfType<Fader>();

            yield return fader.FadeOut(this.fadeOutTime);

            var savingWrapper = GameObject.FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(this.loadSceneNumber);

            yield return new WaitForSeconds(this.fadeWaitTime);

            savingWrapper.Load();

            this.UpdatePlayer(this.GetOtherPortal());

            // シーン移動先のプレイヤーの座標を保存
            savingWrapper.Save();

            yield return fader.FadeIn(this.fadeInTime);

            // 役目が終わったら破棄する
            GameObject.Destroy(this.gameObject);
        }
        private void UpdatePlayer(Portal currentPortal)
        {
            var player = GameObject.FindWithTag("Player");
            var playerNavMeshAgent = player.GetComponent<NavMeshAgent>();

            playerNavMeshAgent.enabled = false;

            player.transform.position = currentPortal.spwan.transform.position;
            player.transform.rotation = currentPortal.transform.rotation;

            playerNavMeshAgent.enabled = true;
        }
        private Portal GetOtherPortal()
        {
            foreach (var portal in GameObject.FindObjectsOfType<Portal>())
            {
                if (this == portal) continue;
                if (portal.destination != this.destination) continue;

                return portal;
            }

            return null;
        }
    }
}

