using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vapor : MonoBehaviour
{
    [SerializeField] float healthDmg;
    [SerializeField] float destroyDelay;
    ParticleSystem[] _particleSystems;
    float timer = 1f;
    float set = 0f; //0 is stopped
    bool inside;
    CharacterDamageController damageController;
    BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        _particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (inside)
        {
            damageController?.TakePoisonDamage(healthDmg);
        }

        timer-=Time.deltaTime * set;

        if (timer < 0 && set !=0)
        {
            for (int i = 0; i < _particleSystems.Length; i++)
            {
                _particleSystems[i].Stop();
            }
            set = 0f;
            boxCollider.enabled = false;
            Destroy(gameObject, destroyDelay);
        }
    }

    public void SetTimer(float duration)
    {
        timer = duration;
        set = 1f;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<CharacterDamageController>(out CharacterDamageController comp))
        {
            inside = true;
            damageController = comp;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (damageController != null)
        {
            inside = false;
            damageController = null;
        }
    }

}
