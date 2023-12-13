using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float knockBack;
    public float damage;

    public float hitstopDuration;
    [SerializeField] LayerMask layerToHit;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (layerToHit == (layerToHit | (1 << other.gameObject.layer)))
        {
            if (other.TryGetComponent<DamageController>(out DamageController comp))
            {
                comp.TakeDamage(damage, knockBack, transform);
            }
        }
    }
}
