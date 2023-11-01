using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int matchCount;
    
    private Fraction player1Fraction;
    private Fraction player2Fraction;
    
    [SerializeField] GameObject field;

    private void Start()
    {
        player1Fraction = Fraction.Attacker;
        player2Fraction = Fraction.Defender;

        OnGameStart();
    }

    private void OnGameStart()
    {
        GameplayEvents.OnGameStart();
    }

    private void OnGameEnd()
    {
        GameplayEvents.OnGameEnd();
    }

    private void ResetGame()
    {
        // Delete Players

        // Delete Ball
    }

    private void SwitchSide()
    {
        // Rotate Camera

        if (player1Fraction == Fraction.Attacker)
        {
            player1Fraction = Fraction.Defender;
            player2Fraction = Fraction.Attacker;
        }
        else
        {
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
}
