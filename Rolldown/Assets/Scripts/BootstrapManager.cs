using System.Collections;
using System.Collections.Generic;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Connection;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using HeathenEngineering.SteamworksIntegration;
using FishySteamworks;

public class BootstrapManager : MonoBehaviour
{
    public static BootstrapManager instance;
    [SerializeField] public LobbyManager lobbyManager;

    private void Awake() => instance = this;
    

    [SerializeField] private string menuName = "MainMenu";

    private void Start()
    {
    }

    public void GoToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(menuName, LoadSceneMode.Additive);
    }

    
    
}
