using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControllRemover : MonoBehaviour
    {
        private PlayableDirector timeline = null;
        private GameObject player = null;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            this.timeline = this.GetComponent<PlayableDirector>();
            Debug.Assert(this.timeline != null);

        }
        private void OnEnable()
        {
            this.timeline.played += this.DisableControl;
            this.timeline.stopped += this.EnableControl;
        }

        private void OnDisable()
        {
            this.timeline.played -= this.DisableControl;
            this.timeline.stopped -= this.EnableControl;
        }

        private void DisableControl(PlayableDirector pd)
        {
            this.player = GameObject.FindWithTag("Player");
            this.player.GetComponent<ActionScheduler>().CancelCurrentAction();
            this.player.GetComponent<PlayerController>().enabled = false;
        }
        private void EnableControl(PlayableDirector pd)
        {
            this.player.GetComponent<PlayerController>().enabled = true;
        }
    }
}