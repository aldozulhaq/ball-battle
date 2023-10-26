using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameplayEvents
{
    public static event Action OnPlayerSpawnE;
    public static event Action OnClickDefenderFieldE;
    public static event Action OnClickAttackerFieldE;
    public static void OnPlayerSpawn()
    {
        OnPlayerSpawnE?.Invoke();
    }
}
