using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Parry Data")]
public class ParryData : ScriptableObject
{
    public AnimationCurve animationCurve;
    public float parrySpeed;
}
