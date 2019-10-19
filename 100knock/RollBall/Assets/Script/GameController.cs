using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject winnerGameObject = null;
    public PlayerController playerController = null;
    public UIController uiController = null;

    private State state = State.Title;

    public enum State
    {
        Title = 0,
        Game,
    }

    // Use this for initialization
    private void Start()
    {
        this.SetState(State.Title);
    }

    private void SetState(State in_state)
    {
        this.state = in_state;
        this.uiController.SetState(in_state);
        this.playerController.SetState(in_state);
    }
}