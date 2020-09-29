using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup canvasGropu = null;
        private Coroutine currentRoutine = null;

        public void FadeOutImmediate()
        {
            this.GetCanvasGroup().alpha = 1f;
        }

        public void FadeInImmediate()
        {
            this.GetCanvasGroup().alpha = 0f;
        }

        public Coroutine FadeOut(float time)
        {
            return this.Fade(1f, time);
        }

        public Coroutine FadeIn(float time)
        {
            return this.Fade(0f, time);
        }

        private Coroutine Fade(float target, float time)
        {
            if (this.currentRoutine != null)
            {
                this.StopCoroutine(this.currentRoutine);
                this.currentRoutine = null;
            }

            this.currentRoutine = this.StartCoroutine(this.FadeRoutine(target, time));
            return this.currentRoutine;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while(!Mathf.Approximately(target, this.GetCanvasGroup().alpha))
            {
                this.GetCanvasGroup().alpha = Mathf.MoveTowards(this.GetCanvasGroup().alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Awake()
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

