using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    //  インスペクター上の設定
    public Transform target;

    private Vector3 m_offset = Vector3.zero;

	// Use this for initialization
	void Start () {

        m_offset = GetComponent<Transform>().position - target.position;
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Transform>().position = target.position + m_offset;
	}
}
