using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject Player = null;
    public GameObject Emitter = null;
    public GameObject TitleUI = null;
	
	// Update is called once per frame
	void Update ()
    {
        bool bPlaying = IsPlaying();

        //  ゲームスタートさせる条件がそろっている場合は入力によってゲームスタートする
        if ( bPlaying == true )
        {
            return;
        }

        bool bGameStart = false;
        for( int i = 0; i < Input.touchCount; ++i)
        {
            Touch touch = Input.GetTouch(i);

            if( touch.phase == TouchPhase.Began )
            {
                bGameStart = true;
                break;
            }
        }
		
        if( Input.GetMouseButtonDown(0) )
        {
            bGameStart = true;
        }

        if( bGameStart )
        {
            _GameStart();
        }
	}

    public void GameOver()
    {
        TitleUI.SetActive(true);

        FindObjectOfType<ScoreUI>().Save();
    }

    public bool IsPlaying()
    {
        return (TitleUI.activeSelf == false);
    }

    private void _GameStart()
    {
        GameObject playerGameObject = Instantiate(Player, Player.transform.position, Quaternion.identity);
        Player playerComp = playerGameObject.GetComponent<Player>();
        playerComp.gameManager = this;

        Emitter.SetActive(true);
        TitleUI.SetActive(false);
    }

}
