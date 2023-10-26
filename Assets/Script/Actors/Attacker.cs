using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : Soldier
{
    //Speeds
    [SerializeField] float carryingSpeed;

    private void Start()
    {
        soldierFraction = Fraction.Attacker;
    }
}
