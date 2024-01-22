using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TimeManager : MonoBehaviour
{
    public float tick;
    public float second;
    public int minute;
    public int hour;
    public int day = 1;
    public GameObject volumeObject;
    
    private Volume volume;

    void Start()
    {
        volume = volumeObject.GetComponent<Volume>();
    }


    void FixedUpdate()
    {
        CalculateTime();
        ControlVolume();
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


    private void ControlVolume()
    {
        if (hour >= 21 && hour < 22) // dusk at 21:00 / 9pm    -   until 22:00 / 10pm
        {
            print("aaa");
            volume.weight = (float)minute / 60; // since dusk is 1 hr, we just divide the mins by 60 which will slowly increase from 0 - 1 
        }


        if (hour >= 6 && hour < 7) // Dawn at 6:00 / 6am    -   until 7:00 / 7am
        {
            print("bbbbbb");
            volume.weight = 1 - (float)minute / 60; // we minus 1 because we want it to go from 1 - 0
        }
    }
}
