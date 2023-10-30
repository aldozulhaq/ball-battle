using System.Collections;
using System.Collections.Generic;
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

    public void PassBall(Transform targetFeet)
    {
        onPosession = false;

        StopAllCoroutines();
        MoveBall(targetFeet);

    }
}
