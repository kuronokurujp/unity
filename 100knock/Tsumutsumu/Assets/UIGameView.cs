using UnityEngine;
using UnityEngine.UI;

public class UIGameView : MonoBehaviour
{
    [SerializeField]
    private Text score = null;
    private string scoreText = null;

    public int Score
    {
        set
        {
            this.score.text = string.Format(this.scoreText, value);
        }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        this.scoreText = this.score.text;
    }
}
