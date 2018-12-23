using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb2d;
    Animator animator;
    bool isActive = true;
    bool isCleared = false;
    bool isJumping = false;

    public float jumpVelocity;
    public GameController gameController;

    // 起動時の処理
    void Awake ()
    {
        // コンポーネント取得
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // フレーム更新毎の処理
    void Update ()
    {
        // クリアしたら更新しない
        if (isCleared == true)
        {
            return;
        }

        // Ctrlボタンを押した場合で、規定の高さより低い場合
        if (Input.GetButtonDown("Fire1"))
        {
            Jump();
        }

        // スプライトアニメーションのstatusを更新
        if (isActive == true)
        {
            animator.SetInteger ("status", 1);
        }
        else
        {
            animator.SetInteger ("status", 0);
        }
    }

    // ジャンプ実行処理
    public void Jump ()
    {
        if (isJumping == false)
        {
            rb2d.velocity = new Vector2 (0.0f, jumpVelocity);
            isJumping = true;
        }
    }

    void OnCollisionEnter2D ( Collision2D collision )
    {
        if (collision.gameObject.tag == "Finish")
        {
            gameController.SetGameEnd();
        }
        else if (collision.collider.isTrigger == false)
        {
            isJumping = false;
        }
    }

    // ゴール時のアニメーション切り替え
    public void Goal()
    {
        // スプライトアニメーションのgoalフラグを更新
        animator.SetInteger("status", 2);
    }

    // クリア状態セット
    public void SetCleared()
    {
        isCleared = true;
    }

    // 状態管理
    public void SetActive(bool active)
    {
        isActive = active;
    }
}
