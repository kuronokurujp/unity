using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] GameObject Shot = null;
    [SerializeField] GameObject Exprosion = null;
    
    [SerializeField] int AromorPointMax = 1000;

    GameObject target = null;
    float shotInterval = 0.0f;
    float shotIntervalMax = 1.0f;

    int aromorPoint = 500;

	// Use this for initialization
	void Start ()
    {
        target = GameObject.Find("PlayerTarget");
        aromorPoint = AromorPointMax;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if( Vector3.Distance( target.transform.position, transform.position ) > 30.0f )
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation( target.transform.position - transform.position );
        transform.rotation = Quaternion.Slerp( transform.rotation, targetRotation, Time.deltaTime * 10.0f );

        shotInterval += Time.deltaTime;
        if( shotInterval >= shotIntervalMax )
        {
            GameObject.Instantiate(Shot, transform.position, transform.rotation);
            shotInterval = 0.0f;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if( collision.gameObject.tag == "Shot")
        {
            int damage = collision.gameObject.GetComponent<ShotPlayer>().Damage;

            // プレイヤーの弾と衝突
            /// 弾のダメージ量から引く
            aromorPoint -= damage;
            //Debug.Log("ShotDamage = " + damage);
            if( aromorPoint <= 0 )
            {
                GameObject.Instantiate(Exprosion, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
