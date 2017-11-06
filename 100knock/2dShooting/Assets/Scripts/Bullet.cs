using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    //  インスペクターに設定
    public float speed = 10.0f;
    public float lifeTime = 5.0f;
    public int power = 1;

	// Use this for initialization
	void Start () {
        //  時間差でオブジェクトを削除
        //Destroy(gameObject, lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
    }

    private void OnEnable()
    {
        //  SetActive(false)にすると velocity値がクリアされるので、有効になった段階で再設定している
        GetComponent<Rigidbody2D>().velocity = transform.up.normalized * speed;
    }
}
