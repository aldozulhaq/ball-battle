using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] float ballSpeed;
    private bool onPosession;
    private Attacker carrier;
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

    public IEnumerator MoveBall(Transform attackerFeet)
    {
        Debug.Log("Move Ball");
        onPosession = true;
        while (onPosession)
        {
            transform.position = Vector3.MoveTowards(transform.position, attackerFeet.position, ballSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator PassBall(Transform target)
    {
        Debug.Log("Passing");
        onPosession = false;

        while (Vector3.Distance(target.position, this.transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, 3 * Time.deltaTime);
            Debug.Log(Vector3.Distance(target.position, this.transform.position));

            yield return new WaitForEndOfFrame();
        }

        Debug.Log("End of Pass");
    }

    public void OnGoal()
    {
        Debug.Log("Goal!!!!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            OnGoal();
        }
    }
}