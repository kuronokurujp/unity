using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup canvasGropu = null;
        public void FadeOutImmediate()
        {
            Debug.Log("FadeOutImmediate");
            this.GetCanvasGroup().alpha = 1f;
        }
        public void FadeInImmediate()
        {
            Debug.Log("FadeInImmediate");
            this.GetCanvasGroup().alpha = 0f;
        }
        public IEnumerator FadeOut(float time)
        {
            while(this.GetCanvasGroup().alpha < 1f)
            {
                this.GetCanvasGroup().alpha += Time.deltaTime / time;
                yield return null;
            }
            this.FadeOutImmediate();
        }
        public IEnumerator FadeIn(float time)
        {
            while(this.GetCanvasGroup().alpha > 0f)
            {
                this.GetCanvasGroup().alpha -= Time.deltaTime / time;
                yield return null;
            }
            this.FadeInImmediate();
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            this.canvasGropu = this.GetComponent<CanvasGroup>();
            Debug.Assert(this.canvasGropu != null);
        }
        private CanvasGroup GetCanvasGroup()
        {
            if (this.canvasGropu != null) return this.canvasGropu;

            this.canvasGropu = this.GetComponent<CanvasGroup>();
            Debug.Assert(this.canvasGropu != null);

            return this.canvasGropu;
        }
    }
}

