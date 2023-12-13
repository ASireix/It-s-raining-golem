using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageController : MonoBehaviour
{
    public abstract void TakeDamage(float amount, float force, Transform enemy);

    public abstract void TakePoisonDamage(float amount);
}
