using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    [SerializeField] GameObject ShotPrefab;
    [SerializeField] GameObject Muzzle;
    [SerializeField] GameObject MuzzleFlash;

    float ShotInterval = 0.0f;
    float ShotIntervalMax = 0.25f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        ShotInterval += Time.deltaTime;

        if ( Input.GetButton("Fire1") )
        {
            if( ShotInterval > ShotIntervalMax )
            {
                //  弾を撃つ
                GameObject.Instantiate(ShotPrefab, Muzzle.transform.position, Camera.main.transform.rotation);

                // マズルフラッシュを表示
                GameObject.Instantiate(MuzzleFlash, Muzzle.transform.position, transform.rotation);

                ShotInterval = 0.0f;
            }
        }
	}
}
