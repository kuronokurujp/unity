using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{

    bool isActive = true;

    public int itemSpan;
    public GameObject itemPrefab;
    public GameController gameController;

    // フレーム更新毎の処理
    void Update ()
    {
        // 指定フレーム数毎に実行
        if (isActive == true && Time.frameCount % itemSpan == 0)
        {
            // アイテムのインスタンスを生成
            var item = Instantiate(itemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            item.GetComponent<ScrollItem>().gameController = gameController;
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }
}
