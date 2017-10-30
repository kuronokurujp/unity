using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //  タグでプレイヤーか判定する
        if ( other.CompareTag("Player") )
        {
            //  接触時に自分自身を消す
            Destroy(gameObject);
        }
    }
}
