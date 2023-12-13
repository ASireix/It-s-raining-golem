using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayController : MonoBehaviour
{
    MorphController currentForm;
    UnityEvent<MorphController> onMorphChange = new UnityEvent<MorphController>();
    public float hp { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMorphChange(MorphController newMorph)
    {

    }

    public void TakeDamage(float amount)
    {
        hp -= amount;

        if (hp < 0f)
        {
            hp = 0f;
        }
    }
}
