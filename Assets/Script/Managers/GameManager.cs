using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Fraction
{
    Attacker,
    Defender
}

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject attackerPrefab;
    [SerializeField] GameObject defenderPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit))
                return;

            Field field = hit.transform.GetComponent<Field>();
            if (!field)       // Only if player click on Field
                return;

            Vector3 hitPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);      // Get mouse pos in world point

            if (field.GetFieldFraction() == Fraction.Attacker)
                Instantiate(attackerPrefab, new Vector3(hitPosition.x, attackerPrefab.transform.position.y, hitPosition.z), Quaternion.identity);
            else
                Instantiate(defenderPrefab, new Vector3(hitPosition.x, defenderPrefab.transform.position.y, hitPosition.z), Quaternion.identity);
        }
    }
}
