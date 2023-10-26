using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
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
}
