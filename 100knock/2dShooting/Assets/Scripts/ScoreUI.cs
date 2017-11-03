using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour {

    public UnityEngine.UI.Text UIText_Score = null;
    public UnityEngine.UI.Text UIText_HiScore = null;

    private int mScore = 0;
    private int mHiScore = 0;

    private string hiScoreKey = "highscore";

    public void AddPoint( int point )
    {
        mScore += point;
    }

    public void Save()
    {
        PlayerPrefs.SetInt(hiScoreKey, mHiScore);
        PlayerPrefs.Save();

        _initialized();
    }

    private void Start()
    {
        _initialized();
    }

    private void Update()
    {
        if( mHiScore < mScore )
        {
            mHiScore = mScore;
        }

        UIText_Score.text = mScore.ToString();
        UIText_HiScore.text = "HiScore : " + mHiScore.ToString();
    }

    private void _initialized()
    {
        mScore = 0;

        mHiScore = PlayerPrefs.GetInt(hiScoreKey, 0);
    }
}
