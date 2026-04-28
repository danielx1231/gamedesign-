using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoSingleton<Timer>
{
    float time;
    bool ka;
    public void PlayTimer(float time,Action action)
    {
        StartCoroutine(enumerator(time, action));
    }
    public void StopTime(float time, Action playAction, Action stopAction)
    {
        StartCoroutine(enumerator1(time, playAction, stopAction));
    }
    private IEnumerator enumerator(float time,Action action)
    {
        yield return new WaitForSeconds(time);
        if (action!=null )
        {
            action();
        }
    }
    private IEnumerator enumerator1(float time, Action playAction, Action stopAction)
    {
        playAction();
           yield return new WaitForSeconds(time);
        stopAction();
    }




    public void KaZhen()
    {
        ka = true;
        Time.timeScale = 0.25f;
    }
    private void Update()
    {
        if (ka)
        {
            time += Time.deltaTime;
            if (time>=0.05f)
            {
                time = 0;
                ka = false;
                Time.timeScale = 1f;
            }
        }
    }
}
