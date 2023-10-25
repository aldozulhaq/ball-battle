using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] Fraction fieldFraction;

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
}
