using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Defender : Soldier
{
    [SerializeField] private float defenseRadius;

    private Vector3 startPos;

    // Speeds
    [SerializeField] float returnSpeed;

    private void OnEnable()
    {
        GameplayEvents.OnAttackerStartCarryingE += OnCarrierChange;
    }
    private void OnDisable()
    {
        GameplayEvents.OnAttackerStartCarryingE -= OnCarrierChange;
    }

    private void Start()
    {
        soldierFraction = Fraction.Defender;
        startPos = this.transform.position;
        StartCoroutine(OnSpawning());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentState != SoldierState.Chasing)
            return;

        if (other.gameObject != ballCarrier)
            return;
        
        OnHitCarrier();
        GameplayEvents.OnHitCarrier();
    }

    protected override IEnumerator OnSpawning(System.Action _Callback = null)
    {
        StartCoroutine(base.OnSpawning());

        /*StartCoroutine(base.OnSpawning(() => {
            StartCoroutine(OnInactive());
        }));*/

        yield return null;
    }

    private IEnumerator StandingBy()
    {
        currentState = SoldierState.Standby;
        SetCarrier();

        while (currentState == SoldierState.Standby)
        {

            if (ballCarrier != null && Vector3.Distance(this.transform.position, ballCarrier.transform.position) <= defenseRadius)
            {
                SetCurrentState(SoldierState.Chasing);
                
                StopAllCoroutines();
                StartCoroutine(ChaseCarrier());
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator ChaseCarrier()
    {
        while (currentState == SoldierState.Chasing)
        {
            Move(ballCarrier.transform.position, normalSpeed);

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnCarrierChange()
    {
        if (currentState != SoldierState.Inactive)
            StartCoroutine(StandingBy());
    }

    private void OnHitCarrier()
    {
        SetCurrentState(SoldierState.Inactive);

        StopAllCoroutines();
        StartCoroutine(OnInactive());
    }
   protected override IEnumerator OnInactive(System.Action _Callback = null)
   {

        StartCoroutine(base.OnInactive(() => 
            StartCoroutine(BackToSpawnPosition())
        ));

        yield return null;

   }
 
    protected override void Reactivate()
    {
        base.Reactivate();
        StartCoroutine(StandingBy());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, defenseRadius);
    }

    private IEnumerator BackToSpawnPosition(System.Action _Callback = null)
    {
        while (Vector3.Distance(transform.position, startPos) >= 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, returnSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        _Callback?.Invoke();
    }
}