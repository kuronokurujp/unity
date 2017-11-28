using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour {

    [SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] float attackMoveStopRadius = 5f;

    ThirdPersonCharacter thirdPersonCharacter;
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickPoint;

    bool isInDirectMode = false;

    // Use this for initialization
    void Start () {

        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        currentDestination = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // 特定入力をするとマウスかキーボードでの入力方法を切り替え
        // TODO add to menu
        if(Input.GetKeyDown(KeyCode.G))
        {
            isInDirectMode = !isInDirectMode;
            currentDestination = transform.position;
        }

        if (isInDirectMode)
        {
            ProcessDirectMovement();
        }
        else
        {
            ProcessMouseMovement();
        }
    }

    private void ProcessDirectMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // calculate camera relative direction to move:
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * camForward + h * Camera.main.transform.right;

        thirdPersonCharacter.Move(movement, false, false);
    }

    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0))
        {
            clickPoint = cameraRaycaster.hit.point;
            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Walkable:
                    //  タッチした位置とプレイヤー位置から移動制限範囲を超えた移動先座標を取得
                    //  タッチした位置とプレイヤー位置との長さが制限範囲を超えてない場合は移動しない
                    currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
                    break;

                case Layer.Enemy:
                    currentDestination = ShortDestination(clickPoint, attackMoveStopRadius);
                    break;

                default:
                    print("Unexpected layer found");
                    return;
            }
        }

        // 移動処理
        WalkToDestination();
    }

    private void WalkToDestination()
    {
        var playerToClickPoint = currentDestination - transform.position;
        if (playerToClickPoint.magnitude >= 0.0f)
        {
        }
        else
        {
            playerToClickPoint = Vector3.zero;
        }

        thirdPersonCharacter.Move(playerToClickPoint, false, false);
    }

    // 現在位置から移動先への限界位置を取得
    private Vector3 ShortDestination(Vector3 destination, float shortening)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }

    private void OnDrawGizmos()
    {
        // Draw Gizmos
        Gizmos.color = Color.black;
        Gizmos.DrawLine( transform.position, clickPoint );
        Gizmos.DrawSphere(clickPoint, 0.15f);
        Gizmos.DrawSphere(currentDestination, 0.15f);

        // 攻撃範囲を表示
        Gizmos.color = new Color(255, 0, 0, 0.5f);
        Gizmos.DrawWireSphere( transform.position, attackMoveStopRadius );
    }
}
