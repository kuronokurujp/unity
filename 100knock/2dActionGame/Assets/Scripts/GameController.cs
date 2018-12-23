using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    enum State
    {
        Ready,
        Play,
        Cleared,
        GameEnd
    }

    State state;
    int score;
    float startTime;
	bool isGameEnd = false;

	public PlayerController player;
    public ItemGenerator itemGenerator;

    public Text stateLabel;
    public Text scoreLabel;
    public Text timeLabel;

    public int clearHurdle;
    public GameObject goalPrefab;

    // 起動時の処理
    void Start ()
    {
        Ready();
    }

    // フレーム毎の処理
    void LateUpdate ()
    {
        switch (state)
        {
        case State.Ready:
            if (Input.GetButtonDown("Fire1")) GameStart();
            break;
        case State.Play:
            string pastTime = (Time.time - startTime).ToString("F2");
            if (score >= clearHurdle) Cleared();
            timeLabel.text = "Time:" + pastTime;
            break;
        case State.Cleared:
            if (isGameEnd == true) GameEnd();
            break;
        case State.GameEnd:
            if (Input.GetButtonDown("Fire1")) Reload();
            break;
        }
    }

    // ゲームスタート前の状態
    void Ready ()
    {
        // 状態の更新
        state = State.Ready;

        // スクロール設定したオブジェクトを全て取得
        ScrollObject[] scrollObjects = GameObject.FindObjectsOfType<ScrollObject>();
        // スクロール設定したオブジェクトを全て無効化
        foreach (ScrollObject so in scrollObjects) so.enabled = false;

        // キャラスプライトとアイテム生成を無効化
        player.SetActive(false);
        itemGenerator.SetActive(false);

        // 状態表示更新
        stateLabel.gameObject.SetActive(true);
        stateLabel.text = "Ready";

        // スコア表示更新
        scoreLabel.gameObject.SetActive(true);
        scoreLabel.text = "Heart:" + 0 + "/" + clearHurdle;

        // 時間表示有効化
        timeLabel.gameObject.SetActive(true);
        timeLabel.text = "Time:0.00";
    }

    // ゲームスタート時の処理
    void GameStart ()
    {
        // 状態の更新
        state = State.Play;

        // スクロール設定したオブジェクトを全て取得
        ScrollObject[] scrollObjects = GameObject.FindObjectsOfType<ScrollObject>();
        // スクロール設定したオブジェクトを全て有効化
        foreach (ScrollObject so in scrollObjects) so.enabled = true;

        // キャラスプライトとアイテム生成を有効化
		player.SetActive(true);
		itemGenerator.SetActive(true);

        // 状態表示を無効化
        stateLabel.gameObject.SetActive(false);
        stateLabel.text = "";

        // 時間表示を有効化
        timeLabel.gameObject.SetActive(true);

        startTime = Time.time;
    }

    void Cleared ()
    {
        state = State.Cleared;

        // アイテム生成を無効化
		itemGenerator.SetActive(false);

        // スクロール設定したアイテムを全て取得
        ScrollItem[] scrollItems = GameObject.FindObjectsOfType<ScrollItem>();
        // スクロール設定したアイテムを全て無効化
        foreach (ScrollItem si in scrollItems) Destroy(si.gameObject);

        // ゴールキャラを設置
        var goal = Instantiate(goalPrefab, new Vector3(16.0f, 2.56f, 0), Quaternion.identity);
        BoxCollider2D bc2d = goal.AddComponent<BoxCollider2D> () as BoxCollider2D;
        bc2d.size.Set(3.0f,2.0f);

        // キャラのクリア状態を更新
		player.SetCleared();
    }

    void GameEnd ()
    {
        state = State.GameEnd;

        // スクロール設定したオブジェクトを全て取得
        ScrollObject[] scrollObjects = GameObject.FindObjectsOfType<ScrollObject>();
        // スクロール設定したオブジェクトを全て無効化
        foreach (ScrollObject so in scrollObjects) so.enabled = false;

        // キャラスプライトをクリア状態にする
		player.Goal();

        stateLabel.gameObject.SetActive(true);
        stateLabel.text = "GameClear";
    }

    void Reload ()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void SetGameEnd()
    {
        isGameEnd = true;
    }

    public void IncreaseScore ()
    {
        score++;
        scoreLabel.text = "Heart:" + score + "/" + clearHurdle;
    }
}
