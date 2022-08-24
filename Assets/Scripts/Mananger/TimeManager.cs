using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    DateTime CurrentTime;
    public float timer;
    public float TimeScale;
    public string DateString;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        Instance = this;

        CurrentTime = new DateTime(1, 1, 1);
    }


    private void Update()
    {
        if (timer > 1 * TimeScale)
        {
            CurrentTime = CurrentTime.AddSeconds(1);
            timer = 0;
        }

        timer += Time.deltaTime;


        DateString = CurrentTime.ToString();
    }

    public bool IsDayTime()
    {
        return (CurrentTime.Hour >= 6 && CurrentTime.Hour < 18);
    }

    public void AddHours(int hours)
    {
        CurrentTime = CurrentTime.AddHours(hours);
    }

}

