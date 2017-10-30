using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    // インスペクター上の設定
    public UnityEngine.UI.Text scoreLabel = null;
    public GameObject winnerGameObject = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        int count = GameObject.FindGameObjectsWithTag("item").Length;
        scoreLabel.text = count.ToString();

        if( count <= 0 )
        {
            winnerGameObject.SetActive(true);
        }
    }
}
