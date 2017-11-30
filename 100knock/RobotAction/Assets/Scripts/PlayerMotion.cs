using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour {

    Animator animator;

	// Use this for initialization
	void Start () {

        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if( Input.GetAxis("Horizontal") > 0 )
        {
            animator.SetInteger("Horizontal", 1);
        }
        else if( Input.GetAxis("Horizontal") < 0 )
        {
            animator.SetInteger("Horizontal", -1);
        }
        else
        {
            animator.SetInteger("Horizontal", 0);
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            animator.SetInteger("Vertical", 1);
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            animator.SetInteger("Vertical", -1);
        }
        else
        {
            animator.SetInteger("Vertical", 0);
        }

        animator.SetBool("Jump", Input.GetButton("Jump"));
    }
}
