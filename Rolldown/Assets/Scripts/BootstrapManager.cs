using System.Collections;
using System.Collections.Generic;
using FishNet.Managing;
using FishNet.Managing.Scened;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using HeathenEngineering.SteamworksIntegration;
using FishySteamworks;

public class BootstrapManager : MonoBehaviour
{
    public static BootstrapManager instance;
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
    

    [SerializeField] private string menuName = "MainMenu";
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private FishySteamworks.FishySteamworks _fishySteamworks; 

    public static string hostId;

    private void Start()
    {
    }

    public void StartGame()
    {
        _networkManager.SceneManager.LoadGlobalScenes(new SceneLoadData("Game"));
    }

    public void GoToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(menuName, LoadSceneMode.Additive);
    }

    public void LobbyCreated()
    {
        var user = UserData.Get();
        hostId = user.ToString();

        _fishySteamworks.StartConnection(true);
        _fishySteamworks.StartConnection(false);
    }

    public void LobbyJoined()
    {
        _fishySteamworks.SetClientAddress(hostId);
        _fishySteamworks.StartConnection(false);
    }

    public void LeaveLobby()
    {
        _fishySteamworks.StopConnection(false);
        if (instance._networkManager.IsServerStarted)
            _fishySteamworks.StopConnection(true);
    }
}
