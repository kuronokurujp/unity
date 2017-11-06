using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Spaceship
{
    public GameManager gameManager = null;

    //  機体の共通コンポーネント
    Spaceship spaceshipe = null;

    public override void Move(Vector2 direction)
    {
        //  可変フレームレートを入れる
        Vector2 move = ( direction * spaceshipe.speed ) * Time.deltaTime;
        Vector3 newPosition = new Vector3( transform.position.x + move.x, transform.position.y + move.y, transform.position.z );

        //  カメラのビューポートからワールド座標を取得
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        newPosition.x = Mathf.Clamp(newPosition.x, min.x, max.x);
        newPosition.y = Mathf.Clamp(newPosition.y, min.y, max.y);

        transform.position = newPosition;
    }

    // Startメソッドをコルーチン化
    IEnumerator Start () {

        spaceshipe = GetComponent<Spaceship>();

        while(true)
        {
            if (spaceshipe.Shot(transform) == true)
            {
                GetComponent<AudioSource>().Play();
            }

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

    //  コリジョン判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //  レイヤー名取得
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);

        //  敵の弾の場合は弾を消す
        if(layerName == "Bullet_Enemy")
        {
            //  ヒットしたオブジェクトは消す
            ObjectPool.instance.ReleaseGameObject(collision.gameObject);
            //Destroy(collision.gameObject);
        }

        // 敵の弾 または 敵の場合、自身は消滅
        if( (layerName == "Bullet_Enemy") || ( layerName == "Enemy") )
        {
            //  爆発を発生
            spaceshipe.Explotion();

            gameManager.GameOver();

            //  自分自身も消す
            Destroy(gameObject);
        }
    }
}
