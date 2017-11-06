using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 強制的にRightbody2Dコンポーネントを付ける
[RequireComponent(typeof(Rigidbody2D))]
abstract public class Spaceship : MonoBehaviour {

    //  インスペクターで設定可能
    public float speed = 1.0f;
    public float shotDelay = 1.0f;
    public GameObject bullet = null;
    public GameObject explotion = null;
    public bool canShot = true;

    protected Animator mAnimator = null;

    //  弾を撃つ
    public bool Shot(Transform origin)
    {
        if (canShot == false)
        {
            return false;
        }

        //  プール式に変更してみる
        ObjectPool.instance.GetGameObject(bullet, origin.position, origin.rotation);
        //        Instantiate(bullet, origin.position, origin.rotation);

        return true;
    }

    //  移動する(継承先で実装)
    public abstract void Move(Vector2 direction);

    //  爆発する
    public void Explotion()
    {
        Instantiate(explotion, transform.position, transform.rotation);
    }

    private void Awake()
    {
        mAnimator = GetComponent<Animator>();
    }
}
