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
		
        if( ( IsPlaying() == false ) && (Input.GetKeyDown(KeyCode.X)))
        {
            _GameStart();
        }
	}

    public void GameOver()
    {
        TitleUI.SetActive(true);
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
