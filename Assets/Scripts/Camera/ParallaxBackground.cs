using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParallaxItem
{
    public Transform _transform;
    public Vector3 _startPos;
    public float _parallax;
    public float _length;
    
    public ParallaxItem(Transform transform, Vector3 startPos, float parallax, float length)
    {
        _transform = transform;
        _startPos = startPos;
        _length = length;
        _parallax = parallax;
    }
}

public class ParallaxBackground : MonoBehaviour
{

    [SerializeField] ParallaxItem[] layerBackground; // Set in inspector expect for startpos
    Camera cam;
    [SerializeField] int invert;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        InitLayers();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyParallax();
    }

    void ApplyParallax()
    {
        for (int i = 0; i < layerBackground.Length; i++)
        {
            float dist = cam.transform.position.x * layerBackground[i]._parallax;
            float temp = cam.transform.position.x * (1 - layerBackground[i]._parallax);
            layerBackground[i]._transform.position = new Vector3
                (
                layerBackground[i]._startPos.x + dist, 
                layerBackground[i]._startPos.y, 
                layerBackground[i]._startPos.z
                );
            
            if (temp > layerBackground[i]._startPos.x + layerBackground[i]._length)
            {
                layerBackground[i]._startPos.x += layerBackground[i]._length;
            }
            else if (temp < layerBackground[i]._startPos.x - layerBackground[i]._length)
            {
                layerBackground[i]._startPos.x -= layerBackground[i]._length;
            }
        }
    }

    public void InitLayers()
    {
        layerBackground = new ParallaxItem[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            layerBackground[i] = new ParallaxItem(
                child,
                child.position,
                Mathf.Abs(invert - (float)i/(float)transform.childCount),
                child.GetComponent<SpriteRenderer>().bounds.size.x
                ); 
        }
    }

    public void InvertParallax()
    {
        invert = 1 - invert;

        for (int i = 0; i < layerBackground.Length; i++)
        {
            layerBackground[i]._parallax = Mathf.Abs(invert - (float)i / (float)layerBackground.Length);
        }
    }
}
