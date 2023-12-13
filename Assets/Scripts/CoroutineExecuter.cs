using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineExecuter : MonoBehaviour
{
    public static CoroutineExecuter instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
