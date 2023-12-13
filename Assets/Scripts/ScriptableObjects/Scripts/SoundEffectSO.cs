using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "sfx_sound",menuName = "Audio/SFX SO")]
public class SoundEffectSO : ScriptableObject
{
    [SerializeField] AudioClip[] clips;

    [SerializeField] Vector2 volume = new Vector2(0.5f,0.5f);
    [SerializeField] Vector2 pitch = new Vector2(1f,1f);
    [SerializeField] AudioMixerGroup audioMixer;
    [SerializeField] SoundClipPlayOrder playOrder = SoundClipPlayOrder.Random;
    [SerializeField] int playIndex = 0;

    AudioClip GetClip()
    {
        AudioClip clip = clips[playIndex >= clips.Length ? 0 : playIndex];

        switch (playOrder)
        {
            case SoundClipPlayOrder.Random:
                playIndex = Random.Range(0,clips.Length);
                break;
            case SoundClipPlayOrder.In_Order:
                playIndex = (playIndex + 1) % clips.Length;
                break;
            case SoundClipPlayOrder.Reverse:
                playIndex = (playIndex + clips.Length - 1) % clips.Length;
                break;
        }

        return clip;
    }

    public AudioSource PlaySound(AudioSource audioSourceParam = null)
    {
        if(clips.Length == 0)
        {
            Debug.LogError("No clips to play");
        }
        AudioSource source = audioSourceParam;
        if (source == null)
        {
            var obj = new GameObject("Sound", typeof(AudioSource));
            source = obj.GetComponent<AudioSource>();
        }

        //setup config
        source.clip = GetClip();
        source.volume = Random.Range(volume.x, volume.y);
        source.pitch = Random.Range(pitch.x, pitch.y);
        source.outputAudioMixerGroup = audioMixer;

        source.Play();

        return source;
    }

    enum SoundClipPlayOrder
    {
        Random,
        In_Order,
        Reverse
    }
}
