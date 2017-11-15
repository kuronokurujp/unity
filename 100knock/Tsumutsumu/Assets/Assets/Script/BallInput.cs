using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInput : MonoBehaviour {

    public CreateBall CreateBall = null;

    // 最初にヒットしたボールと最後にヒットしたボールオブジェクト
    GameObject _firstTouchBallObject = null;
    GameObject _lastTouchBallObject = null;

    // 消すボールのリスト
    List<GameObject> _removeBallList = null;
    
    //  リストにボールの名前
    string _currentBallName = "";
        
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
        if( (_firstTouchBallObject == null ) && Input.GetMouseButtonDown(0) )
        {
//            Debug.Log("OnDragStart");
            _OnDragStart();
        }
        else if( Input.GetMouseButtonUp( 0 ))
        {
//            Debug.Log("OnDrawgEnd");
            _OnDragEnd();
        }
        else if (_firstTouchBallObject != null)
        {
//            Debug.Log("OnDragging");
            _OnDragging();
        }
	}

    void _OnDragStart()
    {
        RaycastHit2D hit2D = _GetCurrentTouchHitCollder();
        if( hit2D.collider != null )
        {
            GameObject colObj = hit2D.collider.gameObject;
            if( colObj.name.IndexOf("Ball") != -1 )
            {
                //  名前にBallがある場合はボールオブジェクトと判定
                _removeBallList = new List<GameObject>();
                _firstTouchBallObject = colObj;
                _currentBallName = colObj.name;

                _PushToList(colObj);
            }
        }
    }

    void _OnDragEnd()
    {
        if( _firstTouchBallObject != null )
        {
            int length = _removeBallList.Count;
            if( length >= 3 )
            {
                // 消すリストに3個以降あるならオブジェクトを消す
                for( int i = 0; i < length; ++i )
                {
                    Destroy(_removeBallList[i]);
                }

                CreateBall.DropBall(length);
            }
            else
            {
                for( int i = 0; i < length; ++i )
                {
                    GameObject cureBall = _removeBallList[i];
                    // ﾌﾟﾘﾌｪｯｸｽのdの1文字お消して元の名前に戻す
                    cureBall.name = cureBall.name.Substring(1, cureBall.name.Length - 1);

                    _ChangeAlpha(cureBall, 1.0f);
                }
            }

            _firstTouchBallObject = null;
        }
    }

    void _OnDragging()
    {
        RaycastHit2D col = _GetCurrentTouchHitCollder();
        if( col.collider != null )
        {
            //  ドラッグ中にオブジェクトとヒット
            GameObject colObj = col.collider.gameObject;
            if( colObj.name == _currentBallName )
            {
                //  ヒットしたオブジェクトが最初にタッチしたボール色と同じか
                if( _lastTouchBallObject != colObj )
                {
                    //  最後にタッチしたオブジェクトと現在タッチしたオブジェクトとの距離が
                    //  指定以下なら消すリストに追加
                    float dist = Vector2.Distance(_lastTouchBallObject.transform.position, colObj.transform.position);
                    if( dist <= 1.5f )
                    {
                        //  タッチしたオブジェクトが重複して登録しないよう
                        //  名前を削除用のに変えている
                        _PushToList(colObj);
                    }
                }
            }
        }
    }

    void _ChangeAlpha(GameObject in_obj, float in_alpha)
    {
        SpriteRenderer renderer = in_obj.GetComponent<SpriteRenderer>();
        renderer.color = new Color(1, 1, 1, in_alpha);
    }

    void _PushToList(GameObject in_obj)
    {
        //  最後にタッチしたオブジェクトとする
        _lastTouchBallObject = in_obj;

        //  消すリストに追加
        _removeBallList.Add(in_obj);

        //  消す対象の名前を変える
        //  タッチしたオブジェクトが重複して登録しないよう、名前を削除用のに変えている
        in_obj.name = "d" + in_obj.name;

        _ChangeAlpha(in_obj, 0.5f);
    }

    RaycastHit2D _GetCurrentTouchHitCollder()
    {
        RaycastHit2D hit2D = Physics2D.Raycast( Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        return hit2D;
    }
}
