using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MorphStats", menuName = "Data/MorphStats")]
public class MorphStats : ScriptableObject
{
    public float speedLimit = 5f;
    public float speedMultiplier = 1f;
    public float rigidBodyDrag = 0f;
    public float gScale = 1f;
}
