using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Potion Belt 1", menuName = "Witch/Potion Belt")]
public class PotionBelt : ScriptableObject
{
    public List<PotionWeighted> potions;
}
