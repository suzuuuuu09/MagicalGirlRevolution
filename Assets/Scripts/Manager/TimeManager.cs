using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float tick;
    public float second;
    public int minute;
    public int hour;
    public int day = 1;

    void Start()
    {
        
    }


    void FixedUpdate()
    {
        CalculateTime();
    }


    private void CalculateTime()
    {
        second += Time.fixedDeltaTime * tick;

        if (second >= 60)
        {
            second = 0;
            minute++;
        }

        if (minute >= 60)
        {
            minute = 0;
            hour++;
        }

        if(hour >= 24)
        {
            hour = 0;
            day++;
        }
    }
}
