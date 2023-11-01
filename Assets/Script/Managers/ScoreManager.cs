using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] GameObject winGamePanel;
    [SerializeField] ScoreBoard player1FinalScore;
    [SerializeField] ScoreBoard player2FinalScore;
    [SerializeField] Text winText;
    [SerializeField] Button restartScene;

    private void OnEnable()
    {
        GameplayEvents.OnMatchEndE += OnMatchWin;
    }
    private void OnDisable()
    {
        GameplayEvents.OnMatchEndE -= OnMatchWin;
    }

    private void Awake()
    {
        restartScene.onClick.AddListener(RestartGame);
    }

    private void Start()
    {
        winGamePanel.SetActive(false);
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnMatchWin(Fraction fraction)
    {
        gameManager.GetPlayer_ByFraction(fraction).AddScore();

        CheckWin(gameManager.GetPlayer_ByFraction(fraction));
    }

    private void CheckWin(Player player)
    {
        if (player.GetScore() == 3)
        {
            Player player1 = gameManager.GetPlayer_ByIndex(1);
            Player player2 = gameManager.GetPlayer_ByIndex(2);

            winText.text = player.GetName() + " Win The Game!";
            winGamePanel.SetActive(true);

            player1FinalScore.SetScoreBoard(player1.GetScore(), player1.GetName());
            player2FinalScore.SetScoreBoard(player2.GetScore(), player2.GetName());
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
