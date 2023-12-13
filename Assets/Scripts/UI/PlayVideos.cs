using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayVideos : MonoBehaviour
{
    VideoPlayer player;
    public string fileName;
    private void Awake()
    {
        player = GetComponent<VideoPlayer>();
        player.url = System.IO.Path.Combine(Application.streamingAssetsPath, $"{fileName}.mp4");
        player.Play();
    }
}
