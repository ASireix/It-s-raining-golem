using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class TriggerBorder : MonoBehaviour
{
    Camera cam;
    CinemachineBrain cinemachineBrain;
    CameraBorders cameraBorders;
    BoxCollider2D boxCollider;
    private void Start()
    {
        cam = Camera.main;
        cinemachineBrain = cam.GetComponent<CinemachineBrain>();
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
    }

    public void StopCamera(CameraBorders ca)
    {
        cinemachineBrain.enabled = false;
        ca.stopped = true;
        cameraBorders = ca;
        boxCollider.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Someone named " + collision.transform.root.gameObject.name + " Collided with the trigger and had the tag : "+ collision.transform.root.gameObject.tag);
        if (collision.transform.root.gameObject.CompareTag("Player"))
        {
            cinemachineBrain.enabled = true;
            //cameraBorders.stopped = false;
            boxCollider.enabled = false;
        }
    }
}
