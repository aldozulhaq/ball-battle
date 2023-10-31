using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameplayEvents
{
    public static event Action OnPlayerSpawnE;
    public static event Action OnClickDefenderFieldE;
    public static event Action OnClickAttackerFieldE;
    public static event Action OnAttackerStartCarryingE;
    public static event Action OnHitCarrierE;
    public static event Action OnPassBallE;
    public static event Action OnAttackerWinE;
    public static event Action OnDefenderWinE;

    public static void OnPlayerSpawn()
    {
        OnPlayerSpawnE?.Invoke();
    }

    public static void OnHitCarrier()
    {
        OnHitCarrierE?.Invoke();
    }

    public static void OnAttackerStartCarrying()
    {
        OnAttackerStartCarryingE?.Invoke();
    }

    public static void OnPassBall()
    {
        OnPassBallE?.Invoke();
    }

    public static void OnAttackerWin()
    {
        OnAttackerWinE?.Invoke();
    }

    public static void OnDefenderWin()
    {
        OnDefenderWinE?.Invoke();
    }
}
