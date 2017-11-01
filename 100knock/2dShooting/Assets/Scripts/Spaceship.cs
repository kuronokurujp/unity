using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 強制的にRightbody2Dコンポーネントを付ける
[RequireComponent(typeof(Rigidbody2D))]
public class Spaceship : MonoBehaviour {

    public float speed = 1.0f;

    public float shotDelay = 1.0f;

    public GameObject bullet = null;

    public bool canShot = true;

    public void Shot(Transform origin)
    {
        Instantiate(bullet, origin.position, origin.rotation);
    }

    public void Move(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }
}
