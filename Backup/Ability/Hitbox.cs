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
        switch (layerToHit)
        {
            case int n when n ==LayerMask.NameToLayer("Enemy"):
                if (other.TryGetComponent<Enemy>(out Enemy comp))
                {
                    comp.TakeDamage(knockBack, damage, transform);
                }
                break;
            case int n when n == LayerMask.NameToLayer("Player"):
                if (other.TryGetComponent<Enemy>(out Enemy comp))
                {
                    comp.TakeDamage(knockBack, damage, transform);
                }
                break;
            default:
                break;
        }

        if (layerToHit == LayerMask.NameToLayer("Enemy"))
        {
            
        }
        else
        {
            
        }
        
    }
}
