using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /*
     * @brief オブジェクトがカメラ範囲外になると呼ばれるらしい
     */
    private void OnBecameInvisible()
    {
        Debug.Log("GameObject output CameraView");  
    }
}
