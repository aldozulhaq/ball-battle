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
    [SerializeField] GameObject attackerPrefab;
    [SerializeField] GameObject defenderPrefab;

    Vector3 hitPosition;

    private void Start()
    {
        GameplayEvents.CheckEnergy += CheckHaveEnergy;
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
}
