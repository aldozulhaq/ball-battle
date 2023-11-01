using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int matchCount;
    private int playerReadyCount;
    
    private Fraction player1Fraction;
    private Fraction player2Fraction;

    [Header("Fraction Color")]
    [SerializeField] public Color player1Color;
    [SerializeField] public Color player2Color;

    [SerializeField] GameObject field;
    GameObject mainCamera;

    [Header("UI")]
    [SerializeField] Button readyButton;
    [SerializeField] GameObject readyPanel;
    [SerializeField] Text matchText;

    GameObject winPanel;
    [SerializeField] GameObject attackerWinPanel;
    [SerializeField] GameObject defenderWinPanel;

    private void OnEnable()
    {
        GameplayEvents.OnGameEndE += OnGameEnd;
    }

    private void OnDisable()
    {
        GameplayEvents.OnGameEndE -= OnGameEnd;
    }

    private void Awake()
    {
        readyButton.onClick.AddListener(OnGameStart);


        foreach (Field f in FindObjectsOfType<Field>())
        {
            if (f.GetFieldFraction() == Fraction.Attacker)
            {
                field = f.gameObject;
            }
        }
    }

    private void Start()
    {
        mainCamera = Camera.main.gameObject;
        winPanel = attackerWinPanel.transform.parent.gameObject;

        player1Fraction = Fraction.Attacker;
        player2Fraction = Fraction.Defender;

        matchCount = 1; 
        ShowReadyPanel();
    }

    public void OnGameStart()
    {
        readyPanel.SetActive(false);
        
        GameplayEvents.OnGameStart();
        GameplayEvents.SetPlayerColor(player1Color, player2Color);
    }

    private void OnGameEnd(Fraction winningFraction)
    {
        winPanel.SetActive(true);
        defenderWinPanel.SetActive(false);
        attackerWinPanel.SetActive(false);


        if (winningFraction == Fraction.Attacker)
        {
            attackerWinPanel.SetActive(true);
        }
        else
        {
            defenderWinPanel.SetActive(true);
        }
    }

    private void ResetGame()
    {
        GameplayEvents.OnReset();

        // Deactive winPanel
        winPanel.SetActive(false);
    }

    public void ContinueGame()
    {
        ResetGame();

        SwitchSide();

        matchCount++;
        ShowReadyPanel();
    }

    private void SwitchSide()
    {

        if (player1Fraction == Fraction.Attacker)
        {
            mainCamera.transform.eulerAngles = new Vector3(mainCamera.transform.eulerAngles.x, 180f, mainCamera.transform.eulerAngles.z);
            player1Fraction = Fraction.Defender;
            player2Fraction = Fraction.Attacker;
        }
        else
        {
            mainCamera.transform.eulerAngles = new Vector3(mainCamera.transform.eulerAngles.x, 0f, mainCamera.transform.eulerAngles.z);
            player1Fraction = Fraction.Attacker;
            player2Fraction = Fraction.Defender;
        }
    }

    public int GetMatchCount()
    {
        return matchCount;
    }

    public Vector3 GetFieldSize()
    {
        return field.GetComponent<BoxCollider>().bounds.size;
    }

    public Vector3 RandomizeFieldSpot()
    {
        // Center of field is 0, 0, 0
        float fieldWidth = field.GetComponent<BoxCollider>().bounds.size.x;
        float fieldhHeight = field.GetComponent<BoxCollider>().bounds.size.z;

        float widthMin = 0 - fieldWidth / 2;
        float widthMax = fieldWidth / 2;
        float heightMin = 0 - fieldhHeight / 2;
        float heightMax = fieldhHeight / 2;

        float randomizeWidth = Random.Range(widthMin, widthMax);
        float randomizeHeight = Random.Range(heightMin, heightMax);

        Vector3 randomSpot = new Vector3(randomizeWidth, 0.3f, randomizeHeight);
        Debug.Log(randomSpot);

        return randomSpot;
    }

    private void ShowReadyPanel()
    {
        readyPanel.SetActive(true);
        matchText.text = "Match " + matchCount.ToString();
    }
}
