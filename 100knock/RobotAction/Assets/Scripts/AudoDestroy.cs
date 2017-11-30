using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudoDestroy : MonoBehaviour {

    [SerializeField] float DestroyTime = 0.1f;

    // Use this for initialization
    void Start () {
        Destroy(gameObject, DestroyTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
