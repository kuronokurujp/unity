using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    public float speed = 0.1f;

    private void Start()
    {
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        Vector2 scale = max * 2;

        transform.localScale = scale;
    }

    // Update is called once per frame
    void Update () {

        float y = Mathf.Repeat(Time.time * speed, 1);

        Vector2 offset = new Vector2(0, y);

        GetComponent<MeshRenderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);

	}
}
