using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXHandler : MonoBehaviour
{
    public float timeToDestroy = 1f;

    private void Awake()
    {
        timeToDestroy = GetComponent<ParticleSystem>().main.duration;
        StartCoroutine(DestroySelfAfterSeconds(timeToDestroy));
    }

    IEnumerator DestroySelfAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
