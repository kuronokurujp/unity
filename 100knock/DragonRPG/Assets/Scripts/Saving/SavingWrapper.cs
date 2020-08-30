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
        private void Awake()
        {
            this.CatchSavingSystemComponent();
            Debug.Assert(this.savingSystemComponent);
            Debug.Assert(this.saveFileName != string.Empty);
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private IEnumerator Start()
        {
            this.CatchSavingSystemComponent();
            var fader = GameObject.FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return this.savingSystemComponent.LoadLastScene(this.saveFileName);
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

