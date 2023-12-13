using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image playerHealthImg;
    [SerializeField] Slider bossProgressSlider;

    public void UpdateHealth(float amount)
    {
        playerHealthImg.fillAmount = amount;
    }

    public void UpdateBossProgress(float amount)
    {
        bossProgressSlider.value = amount;
    }

    private void OnEnable()
    {
        BossProgress.onProgressChanged.AddListener(UpdateBossProgress);
    }

    private void OnDisable()
    {
        BossProgress.onProgressChanged.RemoveListener(UpdateBossProgress);
    }
}
