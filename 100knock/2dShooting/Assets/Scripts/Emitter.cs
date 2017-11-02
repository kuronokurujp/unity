using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour {

    public GameObject[] waves;
    public GameManager gameManaer;

    private int mCurrentWave = 0;

	// Use this for initialization
	IEnumerator Start () {
		
        if( waves.Length == 0 )
        {
            yield break;
        }

        while( true )
        {
            while(gameManaer.IsPlaying() == false )
            {
                yield return new WaitForEndOfFrame();
            }

            //  作成
            GameObject wave = Instantiate(waves[mCurrentWave], transform.position, Quaternion.identity);

            //  オブジェクトの管理化にする
            wave.transform.parent = transform;

            //  作成したオブジェクトの子がなくなるまで続ける
            while( wave.transform.childCount != 0 )
            {
                yield return new WaitForEndOfFrame();
            }

            //  子のオブジェクトがなくなったら親を消す
            Destroy(wave);

            mCurrentWave += 1;
            if(waves.Length <= mCurrentWave)
            {
                mCurrentWave = 0;
            }
        }
    }
	
}
