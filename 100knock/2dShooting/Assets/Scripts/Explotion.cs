using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explotion : MonoBehaviour {

    private void OnAnimationFinish()
    {
        Destroy(gameObject);
    }
}
