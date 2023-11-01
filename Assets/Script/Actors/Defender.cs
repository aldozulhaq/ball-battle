using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Defender : Soldier
{
    [SerializeField] private float defenseRadius;

    private Vector3 startPos;
    private GameManager gameManager;

    // Speeds
    [SerializeField] float returnSpeed;
    [SerializeField] GameObject radius;
    float radiusMaxScale;
    float radiusMinScale;

    private void OnEnable()
    {
        GameplayEvents.OnAttackerStartCarryingE += OnCarrierChange;

        GameplayEvents.OnMatchEndE += OnEndGame;
        GameplayEvents.OnResetE += OnReset;
    }
    private void OnDisable()
    {
        GameplayEvents.OnAttackerStartCarryingE -= OnCarrierChange;

        GameplayEvents.OnMatchEndE -= OnEndGame;
        GameplayEvents.OnResetE -= OnReset;
    }

    private void Start()
    {
        radiusMaxScale = 1.3f;
        radiusMinScale = radiusMaxScale / 10;

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
        StartCoroutine(AnimateRadiusToMin());
        StartCoroutine(base.OnInactive(() => 
            StartCoroutine(BackToSpawnPosition())
        ));

        yield return null;
   }
 
    protected override void Reactivate()
    {
        base.Reactivate();
        StartCoroutine(AnimateRadiusToMax());
        StartCoroutine(StandingBy());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, defenseRadius);
    }

    private IEnumerator AnimateRadiusToMax()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(radiusMinScale, radiusMaxScale, t);
            radius.transform.localScale = new Vector3(scale, scale, scale);

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator AnimateRadiusToMin()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime;
            float scale = Mathf.SmoothStep(radiusMaxScale, radiusMinScale, t);
            radius.transform.localScale = new Vector3(scale, scale, scale);

            yield return new WaitForEndOfFrame();
        }
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