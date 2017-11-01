using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    Spaceship spaceship;

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
}
