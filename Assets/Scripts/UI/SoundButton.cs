using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    Slider slider;

    [SerializeField] bool sfx_Get;

    private void Start()
    {
        slider = GetComponent<Slider>();
        if (sfx_Get)
        {
            slider.value = SoundManager.instance.GetSFXVolume();
        }
        else
        {
            slider.value = SoundManager.instance.GetMusicVolume();
        }
    }

    public void UpdateMusic(float value)
    {
        SoundManager.instance.UpdateMusicVolume(value);
    }

    public void UpdateSFX(float value)
    {
        SoundManager.instance.UpdateSFXVolume(value);
    }

    public void TriggerSoundTest()
    {
        SoundManager.instance.PlaySFXTest();
    }
}
