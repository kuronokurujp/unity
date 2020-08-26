using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private PlayableDirector timeline = null;
        private bool alreadyTriggered = false;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            this.timeline = this.GetComponent<PlayableDirector>();
            Debug.Assert(this.timeline != null);
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (this.alreadyTriggered) return;

            if (other.GetComponent<Terrain>()) return;

            this.timeline.Play();
            this.alreadyTriggered = true;
        }
    }   
}