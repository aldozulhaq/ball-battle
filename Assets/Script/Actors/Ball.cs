using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] float ballSpeed;
    private bool onPosession;

    #region Singleton

    public static Ball instance;
    private void Awake()
    {
        instance = this;
    }

    #endregion

    /*private void OnEnable()
    {
        GameplayEvents.OnHitCarrierE += OnAttackerGotHit;
    }

    private void OnDisable()
    {
        GameplayEvents.OnHitCarrierE -= OnAttackerGotHit;
    }*/

    private void SpawnInRandomLocation()
    {

    }

    private void OnTouchAttacker()
    {

    }

    public IEnumerator MoveBall(Transform attackerFeet)
    {
        onPosession = true;
        while (onPosession)
        {
            transform.position = Vector3.MoveTowards(transform.position, attackerFeet.position, ballSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnAttackerGotHit()
    {
        onPosession = false;
        StopAllCoroutines();
    }

    public IEnumerator PassBall(Transform targetFeet)
    {
        onPosession = false;

        while (Vector3.Distance(targetFeet.position, this.transform.position) >= 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetFeet.position, 3 * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(MoveBall(targetFeet));
    }
}