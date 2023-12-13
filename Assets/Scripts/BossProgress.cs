using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossProgress : MonoBehaviour
{
    [SerializeField] GameManagerHolder param;
    float progress = 0f;
    float naturalDelayCurrent = 0f;
    int naturalMultiplier = 1;

    [System.NonSerialized]
    public static UnityEvent<float> onProgressChanged = new UnityEvent<float>();

    [System.NonSerialized]
    public static UnityEvent onBossReady = new UnityEvent();

    bool trackBossHP;

    private void Start()
    {
        onProgressChanged?.Invoke(progress / param.gameParameter.bossProgressParam.maxProgress);
    }

    private void Update()
    {
        if (!trackBossHP)
        {
            if (naturalDelayCurrent >= param.gameParameter.bossProgressParam.naturalDelay)
            {
                AddNaturalProgress();
            }
            else
            {
                naturalDelayCurrent += Time.deltaTime;
            }
        }
    }

    void CheckProgress()
    {
        if (progress >= param.gameParameter.bossProgressParam.maxProgress)
        {
            naturalMultiplier = 0;
            trackBossHP = true;
            onBossReady?.Invoke();
        }
    }

    void AddNaturalProgress()
    {
        progress += param.gameParameter.bossProgressParam.naturalProgress * Time.deltaTime * naturalMultiplier;
        onProgressChanged?.Invoke(progress/ param.gameParameter.bossProgressParam.maxProgress);
        CheckProgress();
    }

    public bool AddProgress(float amount)
    {
        progress += amount * param.gameParameter.bossProgressParam.fillSpeedMultiplier;
        naturalDelayCurrent = 0f;
        onProgressChanged?.Invoke(progress / param.gameParameter.bossProgressParam.maxProgress);
        CheckProgress();
        return progress >= param.gameParameter.bossProgressParam.maxProgress;
    }

    public void UpdateBossHP(float amount)
    {
        onProgressChanged?.Invoke(amount);
    }
}
