using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Slash Data")]
public class SlashData : ScriptableObject
{
    public float slashSpeed;

    [Header("KnockBack")]
    public float firstKnockBack;
    public float secondKnockBack;
    public float thirdKnockBack;

    [Header("Dash Distances")]
    public float firstSlashDashForce;
    public float secondSlashDashForce;
    public float thirdSlashDashForce;

    [Header("Damages")]
    public float firstSlashDashDamage;
    public float secondSlashDashDamage;
    public float thirdSlashDashDamage;

    [Header("SFX")]
    public SoundEffectSO sfx_Slash_First;
    public SoundEffectSO sfx_Slash_Second;
    public SoundEffectSO sfx_Slash_Third;
}
