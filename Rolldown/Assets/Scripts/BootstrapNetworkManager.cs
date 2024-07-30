using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Server;
using FishNet.Managing.Client;
using FishNet.Object;
using FishNet.Transporting;
using FishySteamworks;
using HeathenEngineering.SteamworksIntegration;
using UnityEngine;

public class BootstrapNetworkManager : NetworkBehaviour
{
    public static BootstrapNetworkManager instance;

    [SerializeField] public NetworkManager _networkManager;
    [SerializeField] private FishySteamworks.FishySteamworks _fishySteamworks;

    private void Awake()
    {
        instance = this;
        _networkManager.ServerManager.OnServerConnectionState += OnServerConnectionState;
        _networkManager.ClientManager.OnClientConnectionState += OnClientConnectionState;
    }

    private void OnDestroy()
    {
        _networkManager.ServerManager.OnServerConnectionState -= OnServerConnectionState;
        _networkManager.ClientManager.OnClientConnectionState -= OnClientConnectionState;
    }

    private void OnServerConnectionState(ServerConnectionStateArgs obj)
    {
        if (obj.ConnectionState == LocalConnectionState.Started)
        {
            Debug.Log("Server started and listening for connections.");
        }
        else if (obj.ConnectionState == LocalConnectionState.Stopped)
        {
            Debug.Log("Server stopped.");
            MainMenuManager.instance.LeaveLobby();
        }
    }

    private void OnClientConnectionState(ClientConnectionStateArgs obj)
    {
        if (obj.ConnectionState == LocalConnectionState.Started)
        {
            Debug.Log("Client connected to server.");
        }
        else if (obj.ConnectionState == LocalConnectionState.Stopped)
        {
            Debug.Log("Client disconnected from server.");
            MainMenuManager.instance.LeaveLobby();
        }
    }

    public void LobbyCreated(string hostId)
    {
        //_networkManager.ServerManager.StartConnection();
        _fishySteamworks.SetClientAddress(hostId);
        _fishySteamworks.StartConnection(true);
        _fishySteamworks.StartConnection(false);
    }

    public void LobbyJoined(string hostId)
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

    public void ListConnectedClients()
    {
        Dictionary<int, NetworkConnection> clients = InstanceFinder.ServerManager.Clients;

        foreach (KeyValuePair<int, NetworkConnection> client in clients)
        {
            Debug.Log("Client ID: " + client.Key);
        }
    }
}