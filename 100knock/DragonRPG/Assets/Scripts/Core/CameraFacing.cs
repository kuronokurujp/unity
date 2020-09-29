using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        // カメラの座標計算が全て終わった後に設定する
        // update中にやるとスクリプトの更新タイミングによってカメラ方向との同期がずれるから
        private void LateUpdate()
        {
            this.transform.forward = Camera.main.transform.forward;
        }
    }
}

