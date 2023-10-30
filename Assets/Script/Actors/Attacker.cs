using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : Soldier
{
    private Ball ball;
    [SerializeField] private GameObject goalGate;
    [SerializeField] private Transform feet;

    //Speeds
    [SerializeField] float carryingSpeed;

    private void OnEnable()
    {
        GameplayEvents.OnAttackerStartCarryingE += MoveToFence;
    }
    private void OnDisable()
    {
        GameplayEvents.OnAttackerStartCarryingE -= MoveToFence;
    }

    private void Start()
    {
        goalGate = GameObject.FindGameObjectWithTag("Finish");
        soldierFraction = Fraction.Attacker;
        ball = Ball.instance;
        SetCarrier();

        if (ballCarrier)
            StartCoroutine(MoveToFenceCorroutine());
        else
            StartCoroutine(ChaseBall());
    }

    private IEnumerator ChaseBall()
    {
        currentState = SoldierState.Chasing;

        Vector3 ballPosition = new Vector3(ball.transform.position.x, transform.position.y, ball.transform.position.z);
        while (currentState == SoldierState.Chasing)
        {
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
        if (currentState != SoldierState.Chasing)
            return;

        currentState = SoldierState.Dribbling;

        StopAllCoroutines();
        StartCoroutine(CarryBall());

        GameplayEvents.OnAttackerStartCarrying();
        // Ball position to this
        StartCoroutine(ball.MoveBall(feet));
    }

    private void OnCaught()
    {
        // Inactive
        // Pass
    }

    private void OnTouchFence()
    {
        // Destroy
        Destroy(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ball.gameObject)
        {
            OnTouchBall();
        }
    }
}