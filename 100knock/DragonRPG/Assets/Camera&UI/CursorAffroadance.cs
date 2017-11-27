using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffroadance : MonoBehaviour {

    [SerializeField] Texture2D walkTexture = null;
    [SerializeField] Texture2D targetTexture = null;
    [SerializeField] Texture2D unknownTexture = null;
    [SerializeField] Vector2 cursorhotspot = new Vector2(0, 0);

    CameraRaycaster cameraRaycaster;

	// Use this for initialization
	void Start () {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.onLayerChange += OnDelegateCalled;
	}


    //  update終了後に呼び出す
    //  なぜならカメラのレイキャストクラスのプロパティ設定前に来るのを避けるため
    private void OnDelegateCalled(Layer newLayer)
    {
        Debug.Log( "Change Layer OnDelegateCalled" );
        switch(newLayer)
        {
            case Layer.Walkable:
                Cursor.SetCursor(walkTexture, cursorhotspot, CursorMode.Auto);

                break;

            case Layer.RaycastEndStop:
                Cursor.SetCursor(unknownTexture, cursorhotspot, CursorMode.Auto);

                break;

            case Layer.Enemy:
                Cursor.SetCursor(targetTexture, cursorhotspot, CursorMode.Auto);

                break;

            default:
                Debug.LogWarning( "cursor show is bug Layer defined missmatch = " + cameraRaycaster.currentLayerHit );
                break;
        }
	}

    // TODO delegateに追加したのを外す対処をする
}
