using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycaster : MonoBehaviour {

    public Layer[] layerPriorities =
    {
        Layer.Enemy,
        Layer.Walkable,
    };

    [SerializeField] float distanceToBackground = 100f;
    Camera viewCamera;

    RaycastHit raycastHit;
    public RaycastHit hit
    {
        get { return raycastHit;  }
    }

    Layer layerHit;
    public Layer currentLayerHit
    {
        get { return layerHit; }
    }

    public delegate void OnLayerChange(Layer newLayer);
    public event OnLayerChange onLayerChange;

    void Start()
    {
        viewCamera = Camera.main;
    }

    void Update()
    {
        //  用意したレイヤー番号とライントレースによる衝突が起きているチェック
        foreach(Layer layer in layerPriorities)
        {
            //  用意したレイヤー番号のオブジェクトと衝突したらプロパティーに結果を設定
            var hit = RaycastForLayer(layer);
            if(hit.HasValue)
            {
                raycastHit = hit.Value;

                if( layerHit != layer )
                {
                    layerHit = layer;
                    onLayerChange(layer);
                }
                return;
            }
        }

        //  衝突していない場合はデフォルト設定
        raycastHit.distance = distanceToBackground;
        layerHit = Layer.RaycastEndStop;
    }

    // 戻り値のデータ型に?をつけると呼び出し側でvar型を用いることでnullか否か .HasValueのプロパティで分かる
    RaycastHit? RaycastForLayer(Layer layer)
    {
        //  レイヤーマスクはビットごとに1/0か立っているのでシフトする必要がある。
        int layerMask = 1 << (int)layer;
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit, distanceToBackground, layerMask);
        if(hasHit)
        {
            return hit;
        }

        return null;
    }
}
