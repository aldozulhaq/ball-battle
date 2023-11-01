using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float maxTimer = 150f;
    float currentTime;

    private void OnEnable()
    {
        GameplayEvents.OnGameStartE += StartCountdown;
        GameplayEvents.OnGameEndE += StopCountdown;
    }
    private void OnDisable()
    {
        GameplayEvents.OnGameStartE -= StartCountdown;
        GameplayEvents.OnGameEndE -= StopCountdown;
    }

    void StartCountdown()
    {
        currentTime = maxTimer;
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            // Update UI
            timerText.text = currentTime.ToString("F0");

            // Update material _progress parameter
            float progress = currentTime / maxTimer;
            GetComponent<Image>().material.SetFloat("_Progress", progress);

            yield return null;
        }

        // Ensure timer is 0
        timerText.text = "0";
        GetComponent<Image>().material.SetFloat("_Progress", 0.0f);

        GameplayEvents.OnTimerEnd();
        GameplayEvents.OnGameEnd(Fraction.Defender);
    }

    private void StopCountdown(Fraction fraction)       // parameter were made only to match the delegate
    {
        StopCoroutine(Countdown());
    }
}
