using System.Collections;
using System.Collections.Generic;
using FishNet.Managing;
using FishNet.Transporting.Tugboat;
using UnityEngine;
using HeathenEngineering.SteamworksIntegration;
using Steamworks;

public class P2PNetworkManager : MonoBehaviour
{
    private NetworkManager networkManager;

    void Awake()
    {
        networkManager = GetComponent<NetworkManager>();
    }

    void Start()
    {
        //SteamworksClient.Instance.Events.LobbyCreated.AddListener(OnSteamLobbyCreated);
        //SteamworksClient.Instance.Events.LobbyEntered.AddListener(OnSteamLobbyJoined);
    }

    public void StartHost()
    {
        networkManager.ServerManager.StartConnection();
        networkManager.ClientManager.StartConnection();
    }

    public void JoinHost(string address)
    {
        networkManager.ClientManager.StartConnection(address);
    }

    private void OnSteamLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult == EResult.k_EResultOK)
        {
            StartHost();
        }
    }

    private void OnSteamLobbyJoined(LobbyEnter_t callback)
    {
        //if (!SteamworksClient.Instance.Lobby.IsOwner)
        //{
        //    string hostAddress = SteamworksClient.Instance.Lobby.Owner.ToString();
        //    JoinHost(hostAddress);
        //}
    }
}

