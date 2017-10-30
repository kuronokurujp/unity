using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //  インスペクター上での設定
    public float speed = 10;

	// Use this for initialization
	void Start () {
		
	}

    //  物理演算が動く前に処理が走る
    private void FixedUpdate()
    {
        //  キーボード入力を受け取る
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //  物理に力を加える
        Rigidbody rightBody = GetComponent<Rigidbody>();
        rightBody.AddForce(x * speed, 0, z * speed);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
