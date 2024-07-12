using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public static string hostId;

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