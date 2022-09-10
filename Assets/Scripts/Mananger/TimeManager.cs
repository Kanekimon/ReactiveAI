using System;
using UnityEngine;


public enum DayTime
{
    Sunrise,
    Morning,
    Afternoon,
    Evening,
    Night
}

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


    public DayTime GetDayTime()
    {
        if (CurrentTime.TimeOfDay >= new TimeSpan(0, 0, 0) && CurrentTime.TimeOfDay < new TimeSpan(6, 0, 0))
            return DayTime.Sunrise;
        else if (CurrentTime.TimeOfDay >= new TimeSpan(6, 0, 0) && CurrentTime.TimeOfDay < new TimeSpan(12, 0, 0))
            return DayTime.Morning;
        else if (CurrentTime.TimeOfDay >= new TimeSpan(12, 0, 0) && CurrentTime.TimeOfDay < new TimeSpan(17, 0, 0))
            return DayTime.Afternoon;
        else if (CurrentTime.TimeOfDay >= new TimeSpan(17, 0, 0) && CurrentTime.TimeOfDay < new TimeSpan(21, 0, 0))
            return DayTime.Evening;
        else
            return DayTime.Night;
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

