using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Managing.Scened;
using FishNet.Managing;
using FishNet.Connection;
using FishNet.Object;
using System;
using UnityEngine.ProBuilder.Shapes;

public class BootstrapSceneManager : MonoBehaviour
{
    public static BootstrapSceneManager instance;

    private Queue<string> sceneLoadQueue = new Queue<string>();
    private Queue<string> sceneUnloadQueue = new Queue<string>();
    private bool isSceneLoading = false;
    
    public NetworkConnection serverClientConnection;

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

    //private void ListSceneObjects()
    //{
    //    if (InstanceFinder.SceneManager != null)
    //    {
    //        foreach (var sceneObject in InstanceFinder.SceneManager.SceneObjects)
    //        {
    //            Debug.Log($"SceneObject: {sceneObject.Name}, SceneId: {sceneObject.SceneId}");
    //        }
    //    }
    //}

    private void OnEnable()
    {
        InstanceFinder.SceneManager.OnLoadEnd += OnSceneLoadEnd;
        InstanceFinder.SceneManager.OnUnloadEnd += OnSceneUnloadEnd;
        InstanceFinder.SceneManager.OnLoadStart += OnSceneLoadStart;
        InstanceFinder.SceneManager.OnUnloadStart += OnSceneUnloadStart;
    }

    private void OnDisable()
    {
        InstanceFinder.SceneManager.OnLoadEnd -= OnSceneLoadEnd;
        InstanceFinder.SceneManager.OnUnloadEnd -= OnSceneUnloadEnd;
        InstanceFinder.SceneManager.OnLoadStart -= OnSceneLoadStart;
        InstanceFinder.SceneManager.OnUnloadStart -= OnSceneUnloadStart;
    }

    private void OnSceneLoadStart(SceneLoadStartEventArgs obj) => isSceneLoading = true;
    private void OnSceneUnloadStart(SceneUnloadStartEventArgs obj) => isSceneLoading = true;

    private void OnSceneLoadEnd(SceneLoadEndEventArgs obj)
    { 
        isSceneLoading = false;
        TryProcessNextScene();
        InstanceFinder.SceneManager.OnLoadStart -= OnSceneLoadStart;
        InstanceFinder.SceneManager.OnLoadEnd -= OnSceneLoadEnd;
    }

    private void OnSceneUnloadEnd(SceneUnloadEndEventArgs obj)
    {
        isSceneLoading = false;
        TryProcessNextScene();
        InstanceFinder.SceneManager.OnUnloadEnd -= OnSceneUnloadEnd;
        InstanceFinder.SceneManager.OnUnloadStart -= OnSceneUnloadStart;
    }

    public void QueueLoadScene(string sceneName)
    {
        sceneLoadQueue.Enqueue(sceneName);
        TryProcessNextScene();
    }

    public void QueueUnloadScene(string sceneName)
    {
        sceneUnloadQueue.Enqueue(sceneName);
        TryProcessNextScene();
    }

    private void TryProcessNextScene()
    {
        if (!isSceneLoading)
        {
            if (sceneUnloadQueue.Count > 0)
            {
                string sceneName = sceneUnloadQueue.Dequeue();
                UnloadScene(sceneName);
            }
            else if (sceneLoadQueue.Count > 0)
            {
                string sceneName = sceneLoadQueue.Dequeue();
                LoadScene(sceneName);
            }
        }
    }

    public void StartGame()
    {
        QueueUnloadScene("MainMenu");
        QueueLoadScene("Game");
    }

    private void LoadScene(string sceneName)
    {
        if (!InstanceFinder.IsServerStarted)
        {
            Debug.LogError("Server not started");
            return;
        }

        if (!isSceneLoading)
        {
            isSceneLoading = true;
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

        if (!isSceneLoading)
        {
            isSceneLoading = true;
            SceneUnloadData unloadData = new SceneUnloadData(sceneName);
            InstanceFinder.SceneManager.UnloadGlobalScenes(unloadData);
        }
    }

    
}
