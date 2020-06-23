/// <summary>
/// 2020/06/16: 
/// データの座標位置が0を基準としたオフセットとは限らない
///             
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

public class PointCloudLoader : MonoBehaviour
{
    [SerializeField]
    private Shader pointCloudShader = null;

    [Range(0, 500)] public float pointRaidus = 100;
    [Range(0, 500)] public float pointSize = 10;

    private ComputeBuffer posBuffer = null;
    private ComputeBuffer colBuffer = null;
    private Material material = null;
    private List<(Vector3, Vector3)> pts;

    public bool IsReady { get; private set; } = false;

    public Vector3 MaxPos { get; private set; } = Vector3.zero;
    public Vector3 MinPos { get; private set; } = Vector3.zero;

    public async Task<bool> Load(TextAsset pointData)
    {
        if (this.pointCloudShader == null)
        {
            return false;
        }

        if (this.pts == null)
        {
            this.pts = await PtsReader.Load(pointData);
        }

        List<Vector3> positions = this.pts.Select(item => item.Item1).ToList();
        List<Vector3> colors = this.pts.Select(item => item.Item2).ToList();

        int size = Marshal.SizeOf(new Vector3());
        this.posBuffer = new ComputeBuffer(positions.Count, size);
        this.colBuffer = new ComputeBuffer(colors.Count, size);
        this.posBuffer.SetData(positions);
        this.colBuffer.SetData(colors);

        this.material = new Material(this.pointCloudShader);
        this.material.SetBuffer("colBuffer", this.colBuffer);
        this.material.SetBuffer("posBuffer", this.posBuffer);

        this.ReflectParams();

        this.MaxPos = new Vector3(positions.Max(v => v.x), positions.Max(v => v.y), positions.Max(v => v.z));
        this.MinPos = new Vector3(positions.Min(v => v.x), positions.Min(v => v.y), positions.Min(v => v.z));

        this.IsReady = true;

        return true;
    }

    public void Unload()
    {
        if (this.IsReady == false)
        {
            return;
        }

        this.posBuffer.Release();
        this.colBuffer.Release();

        this.IsReady = false;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        this.ReflectParams();
    }

    void ReflectParams()
    {
        if (this.material == null)
        {
            return;
        }

        this.material.SetFloat("_Radius", this.pointRaidus);
        this.material.SetFloat("_Size", this.pointSize);
        this.material.SetVector("_WorldPos", this.transform.position);
    }

    /// <summary>
    /// OnRenderObject is called after camera has rendered the scene.
    /// </summary>
    void OnRenderObject()
    {
        if (this.IsReady == false)
        {
            return;
        }

        this.material.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.Points, this.pts.Count);
    }
}
