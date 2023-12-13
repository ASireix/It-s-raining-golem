using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Ability data")]
public class AbilityData : ScriptableObject
{
    public new string name;
    public string description;
    public float mana;
    public float cooldown;
    public int level;


    [Header("Animation")]
    public float speed = 1f;
}
