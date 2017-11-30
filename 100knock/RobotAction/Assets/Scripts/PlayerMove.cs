using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    [SerializeField] float speed = 15.0f;
    [SerializeField] float jumpSpeed = 8.0f;
    [SerializeField] float gravity = 20.0f;
    Vector3 moveDirection = Vector3.zero;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        CharacterController controller = GetComponent<CharacterController>();
        if( controller == null )
        {
            Debug.Log("dont attach CharacterController Component");
            return;
        }

        // hit ground
        if( controller.isGrounded == true )
        {
            // up down x / z
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical") );
            // local space -> world space
            moveDirection = transform.TransformDirection(moveDirection);

            moveDirection *= speed;

            if(Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
	}
}
