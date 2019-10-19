using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // インスペクター上の設定
    [SerializeField]
    private GameObject titleUI = null;

    [SerializeField]
    private GameObject gameUI = null;

    [SerializeField]
    private UnityEngine.UI.Text scoreLabel = null;

    [SerializeField]
    private GameObject winnerGameObject = null;

    public void SetState(GameController.State in_state)
    {
        switch (in_state)
        {
            case GameController.State.Title:
                {
                    this.titleUI.SetActive(true);
                    this.gameUI.SetActive(false);
                    break;
                }
            case GameController.State.Game:
                {
                    this.titleUI.SetActive(false);
                    this.gameUI.SetActive(true);

                    int count = GameObject.FindGameObjectsWithTag("item").Length;
                    scoreLabel.text = count.ToString();

                    if (count <= 0)
                    {
                        this.winnerGameObject.SetActive(true);
                    }

                    break;
                }
        }
    }
}