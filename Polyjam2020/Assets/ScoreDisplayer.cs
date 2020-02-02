using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayer : MonoBehaviour
{
    private Text _text;

    public void Init()
    {
        _text = GetComponentInChildren<Text>();
        ResetDisplayer();
    }

    public void ScaleUp(int score)
    {
        iTween.ScaleTo(gameObject, Vector3.one * (1f + (float)score * 0.1f), 0.25f);
    }

    public void ResetDisplayer()
    {
        transform.localScale = Vector3.one;
        DisplayScore(0);
    }

    public void DisplayScore(int score)
    {
        ScaleUp(score);
        _text.text = score.ToString();
    }

}
