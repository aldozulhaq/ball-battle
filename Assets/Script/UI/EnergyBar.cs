using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        GameplayEvents.CheckEnergy += OnSoldierSpawn;
    }

    private void Update()
    {
        if(currentBar < maxBars)
        {
            timeSinceLastRefill += Time.deltaTime;
            if(timeSinceLastRefill >= refillRate)
            {
                timeSinceLastRefill = 0;
                currentBar = (currentBar < maxBars) ? currentBar + 1 : currentBar;
            }
        }

        // Update the material parameter (_progress) to display the bar.
        float progress = Mathf.Lerp(currentBar / (float)maxBars, (currentBar + 1) / (float)maxBars, timeSinceLastRefill / refillRate);
        GetComponent<Image>().material.SetFloat("_Progress", progress);
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
}
