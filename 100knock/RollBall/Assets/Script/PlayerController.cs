using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //  インスペクター上での設定
    [SerializeField]
    public float speed = 10;

    private Rigidbody rigidBody = null;

    // Use this for initialization
    private void Start()
    {
        this.rigidBody = this.GetComponent<Rigidbody>();

        // ジャイロ設定を有効にする
#if UNITY_EDITOR
#elif UNITY_ANDROID
        Input.gyro.enabled = true;
#endif
    }

    //  物理演算が動く前に処理が走る
    private void FixedUpdate()
    {
        Vector3 forceVec = this._getForce();
        // 強制停止状態かどうか
        if (this._isStopForce())
        {
            this.rigidBody.velocity = Vector3.zero;
        }
        else
        {
            //  物理に力を加える
            this.rigidBody.AddForce(forceVec);
        }
    }

    private Vector3 _getForce()
    {
        Vector3 force = Vector3.zero;

#if UNITY_EDITOR
        //  キーボード入力を受け取る
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        force.x = x * this.speed;
        force.z = z * this.speed;
#elif UNITY_ANDROID
        // ジャイロから重力の下向きのベクトルを取得。水平に置いた場合は、gravityV.zが-9.8になる.
        Vector3 gravityV = Input.gyro.gravity;

        // 外力のベクトルを計算.
        force = this._getGyroVector(gravityV) * this.speed;
#endif
        return force;
    }

    private bool _isStopForce()
    {
#if UNITY_EDITOR
        return false;

#elif UNITY_ANDROID
        // ジャイロから重力の下向きのベクトルを取得。水平に置いた場合は、gravityV.zが-9.8になる.
        Vector3 gravityV = Input.gyro.gravity;

        // 外力のベクトルを計算.
        if (Mathf.Abs(gravityV.y) <= 0.4)
        {
            return true;
        }

        return false;
#endif
    }

    public void SetState(GameController.State in_state)
    {
        switch (in_state)
        {
            case GameController.State.Title:
                {
                    this.enabled = false;
                    break;
                }

            case GameController.State.Game:
                {
                    this.enabled = true;
                    break;
                }
        }
    }

    private Vector3 _getGyroVector(Vector3 vec)
    {
        if (this._isStopForce())
        {
            return Vector3.zero;
        }

        float z = -vec.z - 0.2f;
        z = Mathf.Clamp(z, -1.0f, 1.0f);
        if (Mathf.Abs(z) < 0.1f)
        {
            z = 0.0f;
        }

        return new Vector3(vec.x, 0.0f, z);
    }

    private void OnGUI()
    {
        Vector3 gravityV = Vector3.zero;
#if UNITY_ANDROID
        // ジャイロから重力の下向きのベクトルを取得。水平に置いた場合は、gravityV.zが-9.8になる.
        gravityV = Input.gyro.gravity;

        // 外力のベクトルを計算.
#endif

        GUI.Label(new Rect(0, 0, 100, 100), gravityV.ToString());
        GUI.Label(new Rect(0, 32, 100, 100), this._getGyroVector(gravityV).ToString());
    }
}