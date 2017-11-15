using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInput : MonoBehaviour {

    // 最初にヒットしたボールと最後にヒットしたボールオブジェクト
    GameObject _firstTouchBallObject = null;
    GameObject _lastTouchBallObject = null;

    // 消すボールのリスト
    ArrayList _removeBallList = null;

    //  リストにボールの名前
    string _currentBallName = "";
        
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void _OnDragStart()
    {

    }

    void _PushToList()
    {

    }

    RaycastHit2D _GetCurrentCollder()
    {
        
        return null;
    }
}
