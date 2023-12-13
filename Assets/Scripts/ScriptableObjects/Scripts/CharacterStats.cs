using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Data/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public float speed;
    public float maxHealth;
    public float maxMana;

    public float manaRegenSpeed;
}
