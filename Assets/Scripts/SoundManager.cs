using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using static Unity.VisualScripting.Member;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    ObjectPool<AudioSource> audioSourcesPool;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxTestSource; // used to play an audio fx when updating sound

    [SerializeField] AudioMixer masterMixer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSourcesPool = new ObjectPool<AudioSource>(CreateAudioSource, OnGetFromPool, OnReleaseFromPool);
    }

    public void UpdateMusicVolume(float newValue)
    {
        masterMixer.SetFloat("musicVol", Mathf.Log(newValue) * 20f);
    }

    public void UpdateSFXVolume(float newValue)
    {
        masterMixer.SetFloat("sfxVol", Mathf.Log(newValue) * 20f);
    }

    public float GetMusicVolume()
    {
        masterMixer.GetFloat("musicVol", out float value);
        return Mathf.Exp(value/20f);
    }

    public float GetSFXVolume()
    {
        masterMixer.GetFloat("sfxVol", out float value);
        return Mathf.Exp(value / 20f);
    }

    public void PlaySFXTest()
    {
        sfxTestSource.Play();
    }

    public void PlaySFX(SoundEffectSO soundEffect)
    {
        AudioSource source = null;
        try
        {
            source = soundEffect.PlaySound(audioSourcesPool.Get());
        }
        catch (System.Exception)
        {
            audioSourcesPool = new ObjectPool<AudioSource>(CreateAudioSource, OnGetFromPool, OnReleaseFromPool, OnDestroyFromPool);
            audioSourcesPool.Clear();
            source = soundEffect.PlaySound(audioSourcesPool.Get());
        }
         
        source.playOnAwake = false;

        StartCoroutine(Coro_ReleaseDelayed(source,source.clip.length / source.pitch));
    }

    void PlaySFXDelayed(SoundEffectSO soundEffect, float delay)
    {

    }

    IEnumerator Coro_ReleaseDelayed(AudioSource source, float delay)
    {
        for (float i = 0f; i < 1f; i += Time.deltaTime / delay)
        {
            yield return new WaitForEndOfFrame();
        }

        audioSourcesPool.Release(source);
    }

    AudioSource CreateAudioSource()
    {
        var obj = new GameObject("Sound", typeof(AudioSource));
        return obj.GetComponent<AudioSource>();
    }

    void OnGetFromPool(AudioSource pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    void OnReleaseFromPool(AudioSource pooledObject)
    {
        pooledObject.clip = null;
        pooledObject.gameObject.SetActive(false);
    }

    void OnDestroyFromPool(AudioSource pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }

}
