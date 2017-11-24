using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour {

    [SerializeField] float walkMoveStopRadius = 0.2f;

    ThirdPersonCharacter m_Character;
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;

    // Use this for initialization
    void Start () {

        m_Character = GetComponent<ThirdPersonCharacter>();
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        currentClickTarget = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetMouseButton(0))
        {
            print("Cursor raycast hit" + cameraRaycaster.layerHit);
            switch( cameraRaycaster.layerHit )
            {
                case Layer.Walkable:
                    currentClickTarget = cameraRaycaster.hit.point;
                    break;

                case Layer.Enemy:
                    print("Not moveing to enemy");
                    break;

                default:
                    print("Unexpected layer found");
                    return;
            }
        }

        var playerToClickPoint = currentClickTarget - transform.position;
        if(playerToClickPoint.magnitude >= walkMoveStopRadius )
        {
        }
        else
        {
            playerToClickPoint = Vector3.zero;
        }

        m_Character.Move(playerToClickPoint, false, false);
    }
}
