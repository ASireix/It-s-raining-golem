using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraBorders : MonoBehaviour
{
    [SerializeField] float leftBorderPos;
    [SerializeField] float rightBorderPos;

    [SerializeField] TriggerBorder triggerLeft;
    [SerializeField] TriggerBorder triggerRight;
    public bool stopped = false;
    
    // Update is called once per frame
    void Update()
    {
        if (stopped) { return; }

        if (transform.position.x <= leftBorderPos)
        {
            triggerLeft.StopCamera(this);
        }
        else if (transform.position.x >= rightBorderPos)
        {
            triggerRight.StopCamera(this);
        }

    }
}
