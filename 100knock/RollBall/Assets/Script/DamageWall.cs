using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageWall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if( other.gameObject.CompareTag("Player") )
        {
            //  現在のシーン番号取得
            int sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

            //  現在のシーンを再読み込み
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        }
    }
}
