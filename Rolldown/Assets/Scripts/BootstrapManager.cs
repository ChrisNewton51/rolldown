using System.Collections;
using System.Collections.Generic;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Connection;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Heathen.SteamworksIntegration;
using FishySteamworks;

public class BootstrapManager : MonoBehaviour
{
    public static BootstrapManager instance;

    private void Awake() => instance = this;
    

    [SerializeField] private string menuName = "MainMenu";

    private void Start()
    {
        GoToMenu();
    }

    public void GoToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(menuName, LoadSceneMode.Additive);
    }

    public void ExitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
