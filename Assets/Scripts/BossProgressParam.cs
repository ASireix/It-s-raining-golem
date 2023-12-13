using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Progress Param", menuName = "Data/Boss Progress")]
public class BossProgressParam : ScriptableObject
{
    public float maxProgress = 100f;
    public float fillSpeedMultiplier = 1f;
    public float naturalProgress = 0.6f; // how much the bar grow each seconds
    public float naturalDelay = 15f; // how much time you need to speed without killing an enemy before natural progress kicks in
}
