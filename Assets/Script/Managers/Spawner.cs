using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameplayEvents;

public enum Fraction
{
    Attacker,
    Defender
}

public class Spawner : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    [SerializeField] GameObject attackerPrefab;
    [SerializeField] GameObject defenderPrefab;
    [SerializeField] GameObject ballPrefab;

    Vector3 hitPosition;

    private void OnEnable()
    {
        GameplayEvents.CheckEnergy += CheckHaveEnergy;
        GameplayEvents.OnGameStartE += SpawnBall;
    }

    private void OnDisable()
    {
        GameplayEvents.CheckEnergy -= CheckHaveEnergy;
        GameplayEvents.OnGameStartE -= SpawnBall;
    }

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit))
                return;

            Field field = hit.transform.GetComponent<Field>();
            if (!field)       // Only if player click on Field
                return;

            hitPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);      // Get mouse pos in world point

            GameplayEvents.CheckFractionEnergy(field.GetFieldFraction(), 2);
            //HandleSpawn(field.GetFieldFraction(), hitPosition);
        }
    }

    void CheckHaveEnergy(Fraction fraction, int canSpawn)
    {
        if(canSpawn == 0)
            HandleSpawn(fraction, hitPosition);
    }

    void HandleSpawn(Fraction fraction, Vector3 pos)
    {
        if (fraction == Fraction.Attacker)
        {
            Instantiate(attackerPrefab, new Vector3(pos.x, 0.55f, pos.z), Quaternion.identity);
        }
        else if (fraction == Fraction.Defender)
        {
            Instantiate(defenderPrefab, new Vector3(pos.x, 0.55f, pos.z), Quaternion.identity);
        }
    }

    void SpawnBall()
    {
        Vector3 randomizedPos = gameManager.RandomizeFieldSpot();
        Instantiate(ballPrefab, randomizedPos, Quaternion.identity);
    }
}
