using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UIScoreComponent : MonoBehaviour
{
    Text scoreTxt;
    void Start()
    {
        if (!scoreTxt)
        {
            scoreTxt = GetComponent<Text>();
            scoreTxt.text = "Score: 0";
        }
    }
    private void OnEnable()
    {
        GameManager.OnScoreChangeEvent += DisplayScore;
    }
    void DisplayScore(int score)
    {
        if(!scoreTxt) scoreTxt = GetComponent<Text>();
        scoreTxt.text = "Score:" + score;
    }
    private void OnDisable()
    {
        GameManager.OnScoreChangeEvent -= DisplayScore;
    }
}
