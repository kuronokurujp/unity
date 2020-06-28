using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab = null;

    [SerializeField]
    private Sprite[] ballSpriteArray = { };

    [SerializeField]
    private UIGameView uiView = null;

    private GameObject firstTouchBall = null;
    private GameObject lastTouchBall = null;

    private List<GameObject> ballRemoveList = new List<GameObject>();

    private string currentBallName = null;
    private int score = 0;

    private readonly string ballObjectNamePrefex = "ball_";

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        this.uiView.Score = this.score;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(this.DropBall(50));
    }

    private IEnumerator DropBall(int ball)
    {
        for (int i = 0; i < ball; ++i)
        {
            var pos = new Vector2(Random.Range(-2.0f, 2.0f), 7f);
            var ballObject = GameObject.Instantiate(this.ballPrefab, pos, Quaternion.AngleAxis(Random.Range(-40.0f, 40.0f), Vector3.forward));
            int spriteId = Random.Range(0, this.ballSpriteArray.Length);

            var spriteRender = ballObject.GetComponent<SpriteRenderer>();
            spriteRender.sprite = this.ballSpriteArray[spriteId];

            ballObject.name = string.Format("{1}{0}", spriteId, this.ballObjectNamePrefex);

            yield return new WaitForSeconds(0.5f);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && (this.firstTouchBall == null))
        {
            this.OnDragStart();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            this.OnDragEnd();
        }
        else if (this.firstTouchBall != null)
        {
            this.OnDragging();
        }
    }

    void OnDragStart()
    {
        var mousePosition = Input.mousePosition;
        // マウスのzの初期値が-値でワールド座標値に変換できないのでこちらで設定
        mousePosition.z = 10.0f;
        RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePosition), Vector2.zero);
        if (hit2D.collider == null)
        {
            return;
        }

        var hitObj = hit2D.collider.gameObject;
        if (hitObj.name.StartsWith(this.ballObjectNamePrefex) == false)
        {
            return;
        }

        this.firstTouchBall = hitObj;
        this.lastTouchBall = hitObj;
        this.currentBallName = hitObj.name;

        this.ballRemoveList = new List<GameObject>();
        this.PushToRemoveBall(hitObj);
    }

    private void OnDragging()
    {
        var mousePosition = Input.mousePosition;
        // マウスのzの初期値が-値でワールド座標値に変換できないのでこちらで設定
        mousePosition.z = 10.0f;
        RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePosition), Vector2.zero);
        if (hit2D.collider == null)
        {
            return;
        }

        var hitObj = hit2D.collider.gameObject;

        if (hitObj == this.lastTouchBall)
        {
            return;
        }

        if (hitObj.name.CompareTo(this.currentBallName) != 0)
        {
            return;
        }

        var dist = Vector2.Distance(hitObj.transform.position, this.lastTouchBall.transform.position);
        if (dist > 1.5f)
        {
            return;
        }

        this.lastTouchBall = hitObj;
        this.PushToRemoveBall(hitObj);
    }

    private void OnDragEnd()
    {
        if (this.ballRemoveList.Count >= 3)
        {
            for (int i = 0; i < this.ballRemoveList.Count; ++i)
            {
                GameObject.Destroy(this.ballRemoveList[i]);
            }

            this.score += this.ballRemoveList.Count;
            this.uiView.Score = this.score;

            StartCoroutine(this.DropBall(this.ballRemoveList.Count));
        }
        else
        {
            for (int i = 0; i < this.ballRemoveList.Count; ++i)
            {
                this.ChangeBallSpriteAlpha(this.ballRemoveList[i], 100.0f);
            }

            this.ballRemoveList.Clear();
        }

        this.firstTouchBall = null;
        this.lastTouchBall = null;
        this.currentBallName = null;
    }

    private void PushToRemoveBall(GameObject ball)
    {
        this.ballRemoveList.Add(ball);

        this.ChangeBallSpriteAlpha(ball, 50.0f);
    }

    private void ChangeBallSpriteAlpha(GameObject ball, float alpha)
    {
        var sprite = ball.GetComponent<SpriteRenderer>();
        Debug.Assert(sprite != null);

        var color = sprite.color;
        color.a = alpha / 100.0f;

        sprite.color = color;
    }
}
