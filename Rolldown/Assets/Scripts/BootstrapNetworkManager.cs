using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishySteamworks;
using HeathenEngineering.SteamworksIntegration;
using UnityEngine;

public class BootstrapNetworkManager : NetworkBehaviour
{
    public static BootstrapNetworkManager instance;
    private void Awake() => instance = this;

    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private FishySteamworks.FishySteamworks _fishySteamworks;

    public void LobbyCreated()
    {
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