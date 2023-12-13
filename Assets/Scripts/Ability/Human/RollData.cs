using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Roll Data")]
public class RollData : ScriptableObject
{
    public float rollDistance = 1f;
    public float rollSpeed = 3f;
    public AnimationCurve animationCurve;
    public AnimationClip rollClip;

    public SoundEffectSO sfx_Dash;
}
