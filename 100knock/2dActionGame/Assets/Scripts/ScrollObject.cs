using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollObject : MonoBehaviour
{
    public float speed;
    public float startPosition;
    public float endPosition;

    void Update ()
    {
        transform.Translate (new Vector3 (Time.deltaTime * speed * -1, 0, 0));

        if ( transform.position.x <= endPosition )
        {
            ScrollEnd ();
        }
    }

    void ScrollEnd ()
    {
        transform.Translate (new Vector3 (startPosition - endPosition, 0, 0));
    }
}
