using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Managing.Scened;
using FishNet.Managing;

public class BootstrapSceneManager : MonoBehaviour
{
    public static BootstrapSceneManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        LoadScene("Game");
        UnloadScene("MainMenu");
    }

    private void LoadScene(string sceneName)
    {
        if (!InstanceFinder.IsServerStarted)
        {
            Debug.LogError("Server not started");
            return;
        }
            

        InstanceFinder.SceneManager.LoadGlobalScenes(new SceneLoadData(sceneName));
    }

    private void UnloadScene(string sceneName)
    {
        if (!InstanceFinder.IsServerStarted)
        {
            Debug.LogError("Server not started");
            return;
        }


        InstanceFinder.SceneManager.UnloadGlobalScenes(new SceneUnloadData(sceneName));
    }

    
}
