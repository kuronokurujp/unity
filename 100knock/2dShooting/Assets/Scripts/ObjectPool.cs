using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    private static ObjectPool mInstance = null;

    public static ObjectPool instance {
        get
        {
            if ( mInstance == null )
            {
                mInstance =FindObjectOfType<ObjectPool>();
                if( mInstance == null )
                {
                    mInstance = new GameObject("ObjectPool").AddComponent<ObjectPool>();
                }
            }

            return mInstance;
        }
    }

    private Dictionary<int, List<GameObject>> mPoolObjects = new Dictionary<int, List<GameObject>>();

    public GameObject GetGameObject(GameObject in_prefab, Vector2 in_position, Quaternion in_rotation )
    {
        //  プレハブキーを辞書キーにする（プレハブにキーがあるのが初耳）
        int key = in_prefab.GetInstanceID();

        //  辞書キーが無ければリスト追加
        if( mPoolObjects.ContainsKey(key) == false )
        {
            mPoolObjects.Add(key, new List<GameObject>());
        }

        List<GameObject> gameObjects = mPoolObjects[key];

        GameObject go = null;

        //  リスト内のオブジェクトのアクティブをチェックして未使用なのを探す
        for( int i = 0; i < gameObjects.Count; ++i )
        {
            go = gameObjects[i];

            //  未使用なのがあったら必要なのを設定して返す
            if( go.activeInHierarchy == false )
            {
                go.transform.position = in_position;
                go.transform.rotation = in_rotation;

                go.SetActive(true);

                return go;
            }

        }

        //  未使用なのが一つもないので、新規作成して管理リストに追加する。
        go = (GameObject)Instantiate(in_prefab, in_position, in_rotation);

        go.transform.parent = transform;

        gameObjects.Add(go);

        return go;
    }

    //  GameObjectをリリースする。
    //  未使用設定にするのみで削除まではしない
    public void ReleaseGameObject(GameObject in_releaseGameObject)
    {
        in_releaseGameObject.SetActive(false);
    }
}
