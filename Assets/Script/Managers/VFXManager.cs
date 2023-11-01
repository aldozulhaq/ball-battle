using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public GameObject spawnVFX;
    public GameObject explosionVFX;
    public GameObject sparksVFX;
    public GameObject radiusVFX;

    //setup singelton
    public static VFXManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public GameObject GetSpawnVFX()
    {
        return spawnVFX;
    }

    public GameObject GetExplosionVFX()
    {
        return explosionVFX;
    }

    public GameObject GetSparksVFX()
    {
        return sparksVFX;
    }

    public GameObject GetRadiusVFX()
    {
        return radiusVFX;
    }
}
