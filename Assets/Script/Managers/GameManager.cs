using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player
{
    private Fraction Fraction { get; set; }
    private Color Color { get; set; }
    
    public Player(Fraction fraction, Color color)
    {
        this.Fraction = fraction;
        this.Color = color;
    }

    public Fraction GetFraction()
    {
        return Fraction;
    }

    public void SetFraction(Fraction fraction)
    {
        this.Fraction = fraction;
    }

    public Color GetColor()
    {
        return Color;
    }
}

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

    Player player1, player2;

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

        /*player1Fraction = Fraction.Attacker;
        player2Fraction = Fraction.Defender;*/
        player1 = new Player(Fraction.Attacker, player1Color);
        player2 = new Player(Fraction.Defender, player2Color);

        matchCount = 1; 
        ShowReadyPanel();
    }

    public void OnGameStart()
    {
        readyPanel.SetActive(false);
        
        GameplayEvents.OnGameStart();
        GameplayEvents.SetPlayerColor(GetPlayer_ByFraction(Fraction.Attacker).GetColor(), 
                                        GetPlayer_ByFraction(Fraction.Defender).GetColor());
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
        GameplayEvents.OnContinue();

        ResetGame();

        SwitchSide();

        matchCount++;
        ShowReadyPanel();
    }

    private void SwitchSide()
    {

        if (player1.GetFraction() == Fraction.Attacker)
        {
            mainCamera.transform.eulerAngles = new Vector3(mainCamera.transform.eulerAngles.x, 180f, mainCamera.transform.eulerAngles.z);
            /*player1Fraction = Fraction.Defender;
            player2Fraction = Fraction.Attacker;*/
            player1.SetFraction(Fraction.Defender);
            player2.SetFraction(Fraction.Attacker);

        }
        else
        {
            mainCamera.transform.eulerAngles = new Vector3(mainCamera.transform.eulerAngles.x, 0f, mainCamera.transform.eulerAngles.z);
            player1.SetFraction(Fraction.Attacker);
            player2.SetFraction(Fraction.Defender);
        }

        GameplayEvents.SetPlayerColor(GetPlayer_ByFraction(Fraction.Attacker).GetColor(),
                                        GetPlayer_ByFraction(Fraction.Defender).GetColor());
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
        float heightMin = 3.7f - fieldhHeight / 2;
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

    public Player GetPlayer_ByIndex(int playerIndex)
    {
        if (playerIndex == 1)
            return player1;
        else
            return player2;
    }

    public Player GetPlayer_ByFraction(Fraction fraction)
    {
        if (player1.GetFraction() == fraction)
            return player1;
        else
            return player2;
    }
}
