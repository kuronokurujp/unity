using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {

        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        Destroy(gameObject, particleSystem.main.duration);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
