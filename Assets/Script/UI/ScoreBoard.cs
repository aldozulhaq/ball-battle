using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text nameText;

    public void SetScoreBoard(int score, string name)
    {
        scoreText.text = score.ToString();
        nameText.text = name;
    }
}
