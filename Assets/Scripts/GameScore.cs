using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameScore : MonoBehaviour
{
    TextMeshProUGUI scoreTextUI;

    int score;

    public int Score
    {
        get
        {
            return this.score;
        }
        set
        {
            this.score = value;
            UpdateScoreTextUI();
        }
    }
    void Start()
    {
        //Get the Text UI component of this gameObject
        scoreTextUI = GetComponent<TextMeshProUGUI>();
    }

    
    void UpdateScoreTextUI()
    {
        string scoreStr = string.Format("{0:0000000}", score);
        scoreTextUI.text = scoreStr;
    }
    public int GetScore()
    {
        return score;
    }
    void Update()
    {
        
    }
}
