using System;
using System.Collections;
using System.Collections.Generic;
using RPG.SceneManagement;
using UnityEngine;

namespace RPG.Saving 
{
    [RequireComponent(typeof(SavingSystem))]
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField]
        private string saveFileName = string.Empty;
        private SavingSystem savingSystemComponent = null;

        public string GetSaveFileFullPath()
        {
            this.savingSystemComponent = this.GetComponent<SavingSystem>();
            return this.savingSystemComponent.GetPath(this.saveFileName);
        }

        public void Save()
        {
            this.CatchSavingSystemComponent();
            this.savingSystemComponent.Save(this.saveFileName);
        }

        public void Load()
        {
            this.CatchSavingSystemComponent();
            this.savingSystemComponent.Load(this.saveFileName);
        }

        public void Delete()
        {
            this.CatchSavingSystemComponent();
            this.savingSystemComponent.Delete(this.saveFileName);
        }

        /// <summary>
        /// ゲーム基本システム構築になるのでメソッド呼び出しをしている
        /// </summary>
        private void Awake()
        {
            this.CatchSavingSystemComponent();
            Debug.Assert(this.savingSystemComponent);
            Debug.Assert(this.saveFileName != string.Empty);

            // 初回起動時にシーンロードする
            // Startにすると処理順次第でデータが反映しない可能性があるので
            this.StartCoroutine(this.LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            this.CatchSavingSystemComponent();
            yield return this.savingSystemComponent.LoadLastScene(this.saveFileName);
            // シーン内のFaderオブジェクト取得
            var fader = GameObject.FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(1f);
        }

        private void CatchSavingSystemComponent()
        {
            if (this.savingSystemComponent != null) return;
            this.savingSystemComponent = this.GetComponent<SavingSystem>();
        }
        private void Update()
        {
            // セーブとロードのテスト
            if(Input.GetKeyDown(KeyCode.S))
            {
                this.savingSystemComponent.Save(this.saveFileName);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                this.savingSystemComponent.Load(this.saveFileName);
            }
        }
    }
}

