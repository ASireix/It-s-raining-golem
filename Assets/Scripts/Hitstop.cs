using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitstop: MonoBehaviour
{
    static float fixedDeltaTime;

    public static void TriggerHitstop(float duration)
    {
        if (fixedDeltaTime == 0) 
        {
            fixedDeltaTime = Time.fixedDeltaTime;
        }
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
        CoroutineExecuter.instance.StartCoroutine(WaitTime());
        IEnumerator WaitTime()
        {
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1f;
            Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
        }
    }
}
