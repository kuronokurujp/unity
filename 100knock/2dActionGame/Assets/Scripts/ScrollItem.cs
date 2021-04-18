using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollItem : MonoBehaviour
{

    float initialY;

    public float speed;
    public float startPosition;
    public float endPosition;
    public int maxHeight;
    public int minHeight;

    public GameController gameController;

    // インスタンス生成時の処理
    void Start()
    {
        // ランダムでY座標を取得
        initialY = (float)Random.Range(minHeight,maxHeight);

        // 初期位置に設置
        transform.Translate(startPosition, initialY, 0);
    }

    // フレーム更新毎の処理
    void Update()
    {
        // x座標のみ、速度*経過時間を減算
        transform.Translate(-1 * speed * Time.deltaTime, 0, 0);

        GameObject heart = transform.Find("heart").gameObject;
        heart.transform.Rotate(0,Time.deltaTime * -1 * 80,0);

        // x座標が指定地点を過ぎていたら実行する
        if (transform.position.x <= endPosition)
        {
            ScrollEnd();
        }
    }

    // スクロール終了時の処理
    void ScrollEnd()
    {
        // オブジェクトを破棄する
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (gameController != null)
        {
            gameController.IncreaseScore();
        }

        Destroy(this.gameObject);
    }
}
