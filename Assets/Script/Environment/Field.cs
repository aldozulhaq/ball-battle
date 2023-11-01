using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] Fraction fieldFraction;

    List<GameObject> attackerList = new List<GameObject>();
    List<GameObject> defenderList = new List<GameObject>();

    private void OnEnable()
    {
        FillObjects();

        GameplayEvents.SetPlayerColorE += SetObjectsPlayerColor;
    }

    private void OnDisable()
    {
        GameplayEvents.SetPlayerColorE -= SetObjectsPlayerColor;
    }

    private void SetObjectsPlayerColor(Color player1Color, Color player2color)
    {
        foreach (GameObject obj in attackerList)
        {
            obj.GetComponent<MeshRenderer>().material.color = player1Color;
        }

        foreach (GameObject obj in defenderList)
        {
            obj.GetComponent<MeshRenderer>().material.color = player2color;
        }
    }

    public void SetFieldFraction(Fraction fraction)
    {
        fieldFraction = fraction;
    }

    public Fraction GetFieldFraction()
    {
        return fieldFraction;
    }

    public void SwapFraction()
    {
        if (fieldFraction == Fraction.Attacker)
            SetFieldFraction(Fraction.Defender);
        else
            SetFieldFraction(Fraction.Attacker);
    }

    void FillObjects()
    {
        attackerList.Add(GameObject.FindGameObjectWithTag("AttackerFence"));
        defenderList.Add(GameObject.FindGameObjectWithTag("Fence"));
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("GoalPost"))
        {
            defenderList.Add(obj);
        }
    }
}
