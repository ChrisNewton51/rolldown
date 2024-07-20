using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using TMPro;
using HeathenEngineering.SteamworksIntegration;

public class SteamLobby : MonoBehaviour
{
    [SerializeField] private LobbyManager lobbyManager;
    [SerializeField] private BootstrapNetworkManager bootstrapNetworkManager;

    private void Awake()
    {

    }

    public void OnLobbyCreated(LobbyData lobbyData)
    {
        // UI
        MainMenuManager.instance.OnLobbyCreated(lobbyData);

        // Network
        string hostId = UserData.Get().SteamId.ToString();
        lobbyManager.SetLobbyData("HostID", hostId);
        bootstrapNetworkManager.LobbyCreated(hostId);
    }

    public void OnLobbyJoined(LobbyData lobbyData)
    {
        // UI
        MainMenuManager.instance.OnLobbyJoined(lobbyData);

        // Network
        string hostId = lobbyManager.GetLobbyData("HostID");
        bootstrapNetworkManager.LobbyJoined(hostId);
    }

    public void OtherUserJoin(UserData userData)
    {
        // UI
        MainMenuManager.instance.OtherUserJoin(userData);
    }

    public void OnUserLeft(UserLobbyLeaveData userLeaveData)
    {
        // UI
        MainMenuManager.instance.OnUserLeft(userLeaveData);
    }

    public void LobbyLeave()
    {
        // Network
        BootstrapNetworkManager.instance.LeaveLobby();
    }

    public void InviteFriend()
    {
        // Ensure the Steam overlay is enabled and working
        //SteamFriends.ActivateGameOverlayInviteDialog(currentLobbyID);
    }
}
