using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : Soldier
{
    private Ball ball;
    private GameObject goalGate;

    [SerializeField] private Transform feet;
    [SerializeField] private GameObject highlightObject;

    //Speeds
    [SerializeField] float carryingSpeed;

    private void OnEnable()
    {
        GameplayEvents.OnAttackerStartCarryingE += MoveToFence;
        GameplayEvents.OnAttackerStartCarryingE += SetCarrier;
        GameplayEvents.OnHitCarrierE += RemoveCarrier;

        GameplayEvents.OnMatchEndE += OnEndGame;
        GameplayEvents.OnResetE += OnReset;

    }
    private void OnDisable()
    {
        GameplayEvents.OnAttackerStartCarryingE -= MoveToFence;
        GameplayEvents.OnAttackerStartCarryingE -= SetCarrier;
        GameplayEvents.OnHitCarrierE -= RemoveCarrier;

        GameplayEvents.OnHitCarrierE -= OnCaught;   // OnCaught only subscribe if it's carrying/dribbling ball

        GameplayEvents.OnMatchEndE -= OnEndGame;
        GameplayEvents.OnResetE -= OnReset;

    }

    private void Start()
    {
        goalGate = GameObject.FindGameObjectWithTag("Finish");
        soldierFraction = Fraction.Attacker;
        ball = FindObjectOfType<Ball>();
        SetCarrier();

        StartCoroutine(OnSpawning());
    }

    private IEnumerator ChaseBall()
    {
        currentState = SoldierState.Chasing;

        while (currentState == SoldierState.Chasing)
        {
            if (!ball)
                ball = FindObjectOfType<Ball>();

            Vector3 ballPosition = new Vector3(ball.transform.position.x, transform.position.y, ball.transform.position.z);
            Move(ballPosition, normalSpeed);

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator CarryBall()
    {
        Vector3 goalGatePosition = new Vector3(goalGate.transform.position.x, transform.position.y, goalGate.transform.position.z);
        while (currentState == SoldierState.Dribbling)
        {
            Move(goalGatePosition, carryingSpeed);
            
            yield return new WaitForEndOfFrame();
        }
    }

    private void MoveToFence()
    {
        if (currentState != SoldierState.Chasing)
            return;

        currentState = SoldierState.Standby;
        StartCoroutine(MoveToFenceCorroutine());
    }

    private IEnumerator MoveToFenceCorroutine()
    {
        Vector3 goalFence = new Vector3(transform.position.x, transform.position.y, goalGate.transform.position.z);
        while (currentState == SoldierState.Standby)        // Ball is carried by the other, so attacker move to the fence
        {
            Move(goalFence, normalSpeed);

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTouchBall()
    {
        if (ballCarrier)
            return;

        if (currentState == SoldierState.Inactive)
            return;

        GameplayEvents.OnHitCarrierE += OnCaught;

        currentState = SoldierState.Dribbling;

        StopAllCoroutines();
        StartCoroutine(CarryBall());

        GameplayEvents.OnAttackerStartCarrying();

        highlightObject.SetActive(true);
        // Ball position to this
        StartCoroutine(ball.MoveBall(feet));
    }

    private void OnCaught()
    {
        highlightObject.SetActive(false);

        // Pass
        Pass();

        // Inactive
        StartCoroutine(OnInactive());

        GameplayEvents.OnHitCarrierE -= OnCaught;
    }

    private void OnTouchFence()
    {
        // Destroy
        Instantiate(VFXManager.instance.GetExplosionVFX(), transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void Pass()
    {
        GameplayEvents.OnPassBall();

        if(NearestAlly() == null)
        {
            GameplayEvents.OnMatchEnd(Fraction.Defender);
            Debug.Log("Defender Win!!");
            return;
        }
        StartCoroutine(ball.PassBall(NearestAlly().transform));
        StartCoroutine(NearestAlly().ChaseBall());
    }

    private Attacker NearestAlly()
    {
        //Get all attackers
        Attacker[] attackers = FindObjectsOfType<Attacker>();

        if (attackers.Length < 2)       // if no ally, lose
            return null;

        Attacker nearestAlly = attackers[0];

        // Determine their distance with this
        foreach (Attacker ally in attackers)
        {
            if (ally == this)
                continue;

            if (ally.currentState == SoldierState.Inactive)
                continue;

            if (ally == null)
                continue;

            float nearest = Vector3.Distance(this.transform.position, nearestAlly.transform.position);
            float potentialNearest = Vector3.Distance(this.transform.position, ally.transform.position);
            if (nearest > potentialNearest)
                nearestAlly = ally;
        }

        if (nearestAlly.currentState == SoldierState.Inactive || nearestAlly == this)
            nearestAlly = null;

        // Return the nearest
        return nearestAlly;

    }

    public Transform GetFeet()
    {
        return feet;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ball.gameObject )
        {
            OnTouchBall();
            Debug.Log("Touch ball");
        }

        if (other.gameObject.tag == "Fence")
        {
            OnTouchFence();
        }
    }

    protected override void Reactivate()
    {
        base.Reactivate();

        if (ballCarrier)
        {
            currentState = SoldierState.Standby;
            StartCoroutine(MoveToFenceCorroutine());
        }
        else
            StartCoroutine(ChaseBall());
    }
}