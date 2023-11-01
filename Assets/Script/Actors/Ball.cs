using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] float ballSpeed;
    private bool onPosession;
    private Attacker carrier;

    private void OnEnable()
    {
        GameplayEvents.OnResetE += OnReset;
    }

    private void OnDisable()
    {
        GameplayEvents.OnResetE -= OnReset;
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

            yield return new WaitForEndOfFrame();
        }

        Debug.Log("End of Pass");
    }

    public void OnGoal()
    {
        Debug.Log("Goal!!!!");
        GameplayEvents.OnMatchEnd(Fraction.Attacker);
    }

    private void OnReset()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            OnGoal();
        }
    }


}