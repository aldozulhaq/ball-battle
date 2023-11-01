using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Fraction fraction;
    public int maxBars = 6;
    public int spawnCost = 2;
    public float refillRate = 0.5f;

    float timeSinceLastRefill = 0;
    public float currentBar;

    [SerializeField] Color attackerColor;
    [SerializeField] Color defenderColor;
    private bool isGameRunning;

    private void OnEnable()
    {
        GameplayEvents.CheckEnergy += OnSoldierSpawn;
        GameplayEvents.OnGameStartE += StartFilling;
        GameplayEvents.OnGameEndE += StopFilling;

        GameplayEvents.SetPlayerColorE += SetPlayerColor;
    }
    private void OnDisable()
    {
        GameplayEvents.CheckEnergy -= OnSoldierSpawn;
        GameplayEvents.OnGameStartE -= StartFilling;
        GameplayEvents.OnGameEndE -= StopFilling;

        GameplayEvents.SetPlayerColorE -= SetPlayerColor;
    }

    private void Start()
    {
        StopFilling(fraction);      // it doesn't matter which fraction. parameter here were made just to match the delegate
    }

    private void Update()
    {
        if (!isGameRunning)
            return;

        if(currentBar < maxBars)
        {
            timeSinceLastRefill += Time.deltaTime;
            if(timeSinceLastRefill >= refillRate)
            {
                timeSinceLastRefill = 0;
                currentBar = (currentBar < maxBars) ? currentBar + 1 : currentBar;
            }
        }

        float progress = Mathf.Lerp(currentBar / (float)maxBars, (currentBar + 1) / (float)maxBars, timeSinceLastRefill / refillRate);
        float trueProgressBar = currentBar / (float)maxBars;
        GetComponent<Image>().material.SetFloat("_Progress", progress);
        GetComponent<Image>().material.SetFloat("_True_Progress_Bar", trueProgressBar);
    }

    void SetPlayerColor(Color player1Color, Color player2Color)
    {
        attackerColor = player1Color;
        defenderColor = player2Color;

        if (fraction == Fraction.Attacker)
        {
            GetComponent<Image>().color = attackerColor;
        }
        else
        {
            GetComponent<Image>().color = defenderColor;
        }
    }

    void OnSoldierSpawn(Fraction fraction, int canSpawn)
    {
        if(fraction == this.fraction && canSpawn == 2)
        {
            if(currentBar >= spawnCost)
            {
                currentBar = currentBar - spawnCost;
                GameplayEvents.CheckFractionEnergy(fraction, 0);
            }
            else
            {
                GameplayEvents.CheckFractionEnergy(fraction, 1);
            }
        }
    }

    private void SwitchFraction()
    {
        Image energyImage = GetComponent<Image>();
        if (fraction == Fraction.Attacker)
        {
            fraction = Fraction.Defender;
            energyImage.color = defenderColor;
        }
        else
        {
            fraction = Fraction.Attacker;
            energyImage.color = attackerColor;
        }
    }

    private void StartFilling()
    {
        isGameRunning = true;
    }

    private void StopFilling(Fraction fraction)
    {
        isGameRunning = false;
    }
}
