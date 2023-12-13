using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Game/Camera Shake Reader")]
public class CameraShake : ScriptableObject
{
    public event UnityAction shakeEvent;
}
