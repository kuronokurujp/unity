using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //  機体の共通コンポーネント
    Spaceship spaceshipe = null;

	// Startメソッドをコルーチン化
	IEnumerator Start () {

        spaceshipe = GetComponent<Spaceship>();

        while(true)
        {
            spaceshipe.Shot(transform);
            yield return new WaitForSeconds(spaceshipe.shotDelay);
        }
	}
	
	// Update is called once per frame
	void Update () {

        //  キーボード入力を受け取る（Horizontalは左右キー、Verticalは上下キー）
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // 移動方向ベクトルを取得
        Vector2 direction = new Vector2(x, y).normalized;

        spaceshipe.Move(direction);
	}
}
