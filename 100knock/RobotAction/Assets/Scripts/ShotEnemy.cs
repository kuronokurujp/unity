using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotEnemy : MonoBehaviour {

    [SerializeField] GameObject Explosion;

	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, 2.0f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += transform.forward * Time.deltaTime * 100.0f;
	}

    private void OnCollisionEnter(Collision collision)
    {
        bool bHit = ((collision.gameObject.name == "Terrain") || (collision.gameObject.tag == "Player"));
        if( bHit == true )
        {
            //  Terrinオブジェクトと接触したら消滅
            Destroy(gameObject);

            GameObject.Instantiate(Explosion, transform.position, transform.rotation);
        }
    }
}
