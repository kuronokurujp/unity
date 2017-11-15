using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBall : MonoBehaviour {

    public GameObject ballprefab = null;
    public Sprite[] ballSprites = null;

	// Use this for initialization
	void Start () {

        StartCoroutine(_drawBall(55));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator _drawBall( int count )
    {
        for( int i = 0; i < count; ++i )
        {
            GameObject ball = Instantiate(ballprefab);

            int spriteid = Random.Range(0, 5);
            ball.name = "Ball_" + spriteid;

            //@Q
            //  Random.Range(min, max) とあるがこれは min / max の間をランダムで返す関数で
            //  min / max は含まれないのか？
            //
            //@A
            //  Random.Rangeには型 float / int でのオーバーロードによる2種類がある
            //  float型の場合は min / max は含まれる
            //  しかしint型の場合は含まれない（注意）
            ball.transform.position = new Vector3(Random.Range(-2.0f, 2.0f), 7, 0);
            ball.transform.eulerAngles.Set(0, 0, Random.Range(-40, 40));

            SpriteRenderer renderer = ball.GetComponent<SpriteRenderer>();
            renderer.sprite = ballSprites[spriteid];

            yield return new WaitForSeconds(0.05f);
        }
    }
}
