using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyArea : MonoBehaviour {

    private void Start()
    {
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        Vector2 scale = max * 2;

        GetComponent<BoxCollider2D>().size = scale;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);

        if (layerName == "Bullet_Enemy")
        {
            ObjectPool.instance.ReleaseGameObject(collision.gameObject);
            return;
        }
        else if (layerName == "Bullet_Player")
        {
            Bullet bullet = collision.transform.parent.gameObject.GetComponent<Bullet>();
            ObjectPool.instance.ReleaseGameObject(bullet.gameObject);
            return;
        }

        Destroy(collision.gameObject);
    }
}
