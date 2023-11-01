using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class GameplayEvents
{
    public static event Action OnPlayerSpawnE;
    public static event Action OnAttackerStartCarryingE;
    public static event Action OnHitCarrierE;
    public static event Action OnPassBallE;
    public static event Action OnAttackerWinE;
    public static event Action OnDefenderWinE;
    public static event Action OnTimerEndE;
    public static event Action OnGameStartE;

    public static event Action<Color, Color> SetPlayerColorE;

    public delegate void OnSoldierSpawn(Fraction fraction, int canSpawn); // 0 - can't spawn, 1 - can spawn, 2 - just checking
    public static event OnSoldierSpawn CheckEnergy;

    public delegate void OnGameEndDelegate(Fraction winningFraction);
    public static event OnGameEndDelegate OnGameEndE;
    
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

    public static void CheckFractionEnergy(Fraction fraction, int canSpawn)
    {
        CheckEnergy?.Invoke(fraction, canSpawn);
    }

    public static void OnTimerEnd()
    {
        OnTimerEndE?.Invoke();
    }

    public static void OnGameStart()
    {
        OnGameStartE?.Invoke();
    }

    public static void OnGameEnd(Fraction winningFraction)
    {
        OnGameEndE?.Invoke(winningFraction);
    }

    public static void SetPlayerColor(Color player1Color, Color player2Color)
    {
        SetPlayerColorE?.Invoke(player1Color, player2Color);
    }
}
