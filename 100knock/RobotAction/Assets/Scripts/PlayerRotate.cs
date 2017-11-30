using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour {

    GameObject cameraParent;
    Quaternion defaultCameraRot;
    float timer = 0;

	// Use this for initialization
	void Start ()
    {
        cameraParent = Camera.main.transform.parent.gameObject;
        defaultCameraRot = cameraParent.transform.localRotation;
        timer = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal2"), 0);
        cameraParent.transform.Rotate(Input.GetAxis("Vertical2"), 0, 0);

        if( Input.GetButtonDown("CamReset") )
        {
            timer = 0.5f;
        }

        if (timer > 0.0f)
        {
            cameraParent.transform.localRotation = Quaternion.Slerp(cameraParent.transform.localRotation, defaultCameraRot, Time.deltaTime * 10);
            timer -= Time.deltaTime;
        }
    }
}
