using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    static SceneLoader instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public static void LoadSceneByString(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
}
