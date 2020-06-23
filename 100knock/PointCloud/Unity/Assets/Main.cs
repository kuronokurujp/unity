using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

/// <summary>
/// シーンのメイン処理
/// </summary>
public class Main : MonoBehaviour
{
    [SerializeField]
    private TextAsset ptsFile = null;

    [SerializeField]
    private PointCloudLoader cloudLoader = null;

    [SerializeField]
    private CameraController cameraController = null;

    // Start is called before the first frame update
    async Task Start()
    {
        // 点群のロード
        var flag = await this.cloudLoader.Load(this.ptsFile);

        // カメラ位置を点群座標の真ん中に
        this.cameraController.transform.localPosition = (this.cloudLoader.MaxPos + this.cloudLoader.MinPos) * 0.5f;
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        this.cloudLoader.Unload();
    }
}
