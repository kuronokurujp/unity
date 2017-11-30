using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotPlayer : MonoBehaviour {

    [SerializeField] GameObject Explosion;
    [SerializeField] int DamageMax = 100;

    int damage = 0;
    public int Damage
    {
        get { return damage;  }
    }

	// Use this for initialization
	void Start ()
    {
        damage = DamageMax;
        Destroy(gameObject, 2.0f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += transform.forward * Time.deltaTime * 100.0f;

        damage -= 1;
        if( damage <= 1 )
        {
            damage = 1;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if( collision.gameObject.name == "Terrain")
        {
            //  Terrinオブジェクトと接触したら消滅
            Destroy(gameObject);

            GameObject.Instantiate(Explosion, transform.position, transform.rotation);
        }
        else if( collision.gameObject.tag == "Enemy" )
        {
            Destroy(gameObject);

            GameObject.Instantiate(Explosion, transform.position, transform.rotation);
        }
    }
}
