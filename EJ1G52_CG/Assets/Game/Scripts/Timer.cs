using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerMinutes;
    public TextMeshProUGUI timerSeconds;
    public TextMeshProUGUI timerSeconds100;

    private float startTime;
    private float timerTime;
    private bool isRunning = false;

    void Start()
    {
        TimerReset();
    }

    public void TimerStart()
    {
        if (!isRunning)
        {
            print("START");
            isRunning = true;
            startTime = Time.time - timerTime;
        }
    }

    public void TimerStop()
    {
        if (isRunning)
        {
            print("STOP");
            isRunning = false;
        }
    }

    public void TimerReset()
    {
        print("RESET");
        isRunning = false;
        timerTime = 0;
        timerMinutes.text = timerSeconds.text = timerSeconds100.text = "00";
    }

    void Update()
    {
        if (isRunning)
        {
            timerTime = Time.time - startTime;

            int minutesInt = (int)timerTime / 60;
            int secondsInt = (int)timerTime % 60;
            int seconds100Int = (int)((timerTime - (int)timerTime) * 100);

            timerMinutes.text = (minutesInt < 10) ? "0" + minutesInt : minutesInt.ToString();
            timerSeconds.text = (secondsInt < 10) ? "0" + secondsInt : secondsInt.ToString();
            timerSeconds100.text = (seconds100Int < 10) ? "0" + seconds100Int : seconds100Int.ToString();
        }
    }
}