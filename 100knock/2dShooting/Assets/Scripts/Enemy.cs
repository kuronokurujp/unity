using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Spaceship
{
    public int hp = 10;
    public int Point = 100;

    Spaceship spaceship;

    public override void Move(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    private IEnumerator Start()
    {
        spaceship   = GetComponent<Spaceship>();
        spaceship.Move( transform.up * -1 );

        if(spaceship.canShot == false)
        {
            yield break;
        }

        while(true)
        {
            for( int i = 0; i < transform.childCount; ++i )
            {
                spaceship.Shot(transform.GetChild(i));
            }

            yield return new WaitForSeconds(spaceship.shotDelay);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);

        if( layerName != "Bullet_Player")
        {
            return;
        }

        //  ヒットした弾の親オブジェクトに弾データがある。（めんどうだ）
        Bullet bullet = collision.transform.parent.gameObject.GetComponent<Bullet>();
        hp -= bullet.power;

        Destroy(collision.gameObject);

        if ( hp <= 0 )
        {
            FindObjectOfType<ScoreUI>().AddPoint( Point );

            spaceship.Explotion();

            Destroy(gameObject);
        }
        else
        {
            mAnimator.SetTrigger("Damage");
        }

    }
}
