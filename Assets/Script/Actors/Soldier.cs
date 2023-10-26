using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoldierState
{
    Standby,
    Chasing,
    Dribbling,
    Inactive,

}

public class Soldier : MonoBehaviour
{
    [SerializeField] protected SoldierState currentState;
    protected Fraction soldierFraction;
    protected GameObject ballCarrier;

    [SerializeField] Color inactiveColor;
    [SerializeField] Color activeColor;

    // Durations
    [SerializeField] protected float spawnTime;
    [SerializeField] protected float reactivateTime;

    // Speeds
    [SerializeField] protected float normalSpeed;

    protected virtual void Move(Vector3 target, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.LookAt(target);
    }

    protected virtual IEnumerator OnSpawning()
    {
        // TODO Spawn animation here
        yield return new WaitForSeconds(spawnTime);

    }

    protected virtual IEnumerator OnInactive()
    {
        currentState = SoldierState.Inactive;
        // TODO: Change color 
        ChangeColor(inactiveColor);
        yield return new WaitForSeconds(reactivateTime);

        Reactivate();

    }

    protected virtual void SetCarrier()
    {
        Attacker[] attackers = FindObjectsOfType<Attacker>();
        foreach (Attacker attacker in attackers)
        {
            if (attacker.GetCurrentState() == SoldierState.Dribbling)
            {
                ballCarrier = attacker.gameObject;
                return;
            }
        }

        // no carrier
        ballCarrier = null;

    }

    protected virtual void Reactivate()
    {
        // TODO : Change color here
        ChangeColor(activeColor);
    }

    public SoldierState GetCurrentState()
    {
        return currentState;
    }

    protected void SetCurrentState(SoldierState state)
    {
        currentState = state;
    }

    private void ChangeColor(Color color)
    {
        GetComponent<Renderer>().material.SetColor("_BaseColor", color);
    }
}