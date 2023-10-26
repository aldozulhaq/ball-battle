using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : Soldier
{
    private GameObject ballCarrier;
    [SerializeField] private float defenseRadius;

    private Vector3 startPos;

    // Speeds
    [SerializeField] float returnSpeed;

    private void Start()
    {
        soldierFraction = Fraction.Defender;
        startPos = this.transform.position;
        StartCoroutine(OnSpawning());
    }

    protected override IEnumerator OnSpawning()
    {
        StartCoroutine(base.OnSpawning());
        StartCoroutine(StandingBy());

        yield return null;
    }

    private IEnumerator StandingBy()
    {
        currentState = SoldierState.Standby;
        ballCarrier = GetDribbler();

        while (currentState == SoldierState.Standby)
        {
            if (Vector3.Distance(this.transform.position, ballCarrier.transform.position) <= defenseRadius)
            {
                SetCurrentState(SoldierState.Chasing);
                
                StopAllCoroutines();
                StartCoroutine(ChaseDribbler());
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator ChaseDribbler()
    {
        while (currentState == SoldierState.Chasing)
        {
            Move(ballCarrier);

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDribblerChange()
    {
        StopAllCoroutines();
        ballCarrier = GetDribbler();
        StartCoroutine(ChaseDribbler());
    }

    private void OnHitDribbler()
    {
        SetCurrentState(SoldierState.Inactive);

        StopAllCoroutines();
        StartCoroutine(OnInactive());
    }
   protected override IEnumerator OnInactive()
    {
        // TODO : Return back to spawn pos

        StartCoroutine(base.OnInactive());

        yield return null;

    }
 
    protected override void Reactivate()
    {
        base.Reactivate();
        StartCoroutine(StandingBy());
    }

    private GameObject GetDribbler()
    {
        Attacker[] attackers = FindObjectsOfType<Attacker>();
        foreach (Attacker dribbler in attackers)
        {
            if(dribbler.GetCurrentState() == SoldierState.Dribbling)
                return dribbler.gameObject;
        }
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, defenseRadius);
    }
}
