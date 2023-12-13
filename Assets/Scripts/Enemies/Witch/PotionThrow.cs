using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionThrow : ScriptableObject
{
    public PotionBelt potionBelt;
    public float speed = 2f;
    public float minTorque = -5f;
    public float maxTorque = 5f;

    public virtual void Throw(Vector2 launchPosition, Vector2[] targetPositions)
    {
        
    }
}
