using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour {

    GameObject target = null;

    bool isSearch = false;

	// Use this for initialization
	void Start ()
    {
        isSearch = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Lock"))
        {
            isSearch = !isSearch;
            if (!isSearch)
            {
                target = null;
            }
            else
            {
                target = _FindClosestEnemy();
            }

        }

        if( isSearch == true )
        {
            if (target != null)
            {
                //  オブジェクトをターゲットに方向転換
                {
                    Quaternion targetRoataion = Quaternion.LookRotation(target.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRoataion, Time.deltaTime * 10.0f);
                    transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
                }

                //  カメラをターゲットに方向転換
                {
                    Transform cameraParent = Camera.main.transform.parent;

                    Quaternion targetRoataion = Quaternion.LookRotation(target.transform.position - cameraParent.position);
                    cameraParent.localRotation = Quaternion.Slerp(cameraParent.localRotation, targetRoataion, Time.deltaTime * 10.0f);
                    cameraParent.localRotation = new Quaternion(cameraParent.localRotation.x, 0, 0, cameraParent.localRotation.w);
                }
            }
            else
            {
                target = _FindClosestEnemy();
            }
        }

        if(target != null)
        {
            if (Vector3.Distance(target.transform.position, transform.position) > 100.0f)
            {
                target = null;
            }
        }
    }

    GameObject  _FindClosestEnemy()
    {
        GameObject[] got;

        got = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject closest = null;
        Vector3 position = transform.position;
        float distance = Mathf.Infinity;

        for( int i = 0; i < got.Length; ++i )
        {
            float curDistance = (got[i].transform.position - position).sqrMagnitude;
            if(curDistance < distance)
            {
                closest = got[i];
                distance = curDistance;
            }
        }

        return closest;
    }
}
