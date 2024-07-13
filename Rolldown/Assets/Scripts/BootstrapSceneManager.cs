using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Managing.Scened;
using FishNet.Managing;

public class BootstrapSceneManager : MonoBehaviour
{
    public static BootstrapSceneManager instance;

    private bool isSceneLoadingOrUnloading = false;

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

    private void OnEnable()
    {
        InstanceFinder.SceneManager.OnLoadEnd += OnSceneLoadEnd;
        InstanceFinder.SceneManager.OnUnloadEnd += OnSceneUnloadEnd;
    }

    private void OnDisable()
    {
        InstanceFinder.SceneManager.OnLoadEnd -= OnSceneLoadEnd;
        InstanceFinder.SceneManager.OnUnloadEnd -= OnSceneUnloadEnd;
    }

    private void OnSceneLoadEnd(SceneLoadEndEventArgs obj)
    {
        Debug.Log($"Scene {obj.LoadedScenes} has finished loading.");
        isSceneLoadingOrUnloading = false;
    }

    private void OnSceneUnloadEnd(SceneUnloadEndEventArgs obj)
    {
        Debug.Log($"Scene {obj.UnloadedScenes} has finished unloading.");
        isSceneLoadingOrUnloading = false;
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

        if (!isSceneLoadingOrUnloading)
        {
            isSceneLoadingOrUnloading = true;
            SceneLoadData loadData = new SceneLoadData(sceneName);
            InstanceFinder.SceneManager.LoadGlobalScenes(loadData);
        }
    }

    private void UnloadScene(string sceneName)
    {
        if (!InstanceFinder.IsServerStarted)
        {
            Debug.LogError("Server not started");
            return;
        }

        if (!isSceneLoadingOrUnloading)
        {
            isSceneLoadingOrUnloading = true;
            SceneUnloadData unloadData = new SceneUnloadData(sceneName);
            InstanceFinder.SceneManager.UnloadGlobalScenes(unloadData);
        }
    }

    
}
