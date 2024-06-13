using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using TMPro;

public class SteamLobby : MonoBehaviour
{
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;

    public LobbyUIManager lobbyUIManager; // Reference to your UI manager
    private CSteamID currentLobbyID;

    private void Awake()
    {
        if (!SteamManager.Initialized) { return; }

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public void CreateLobby()
    {
        if (SteamManager.Initialized)
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4);
        }
    }

    private void OnLobbyCreated(LobbyCreated_t result)
    {
        if (result.m_eResult != EResult.k_EResultOK)
        {
            Debug.Log("Failed to create lobby");
            return;
        }

        Debug.Log("Lobby created successfully");
        currentLobbyID = new CSteamID(result.m_ulSteamIDLobby);
        
        SteamMatchmaking.SetLobbyData(currentLobbyID, "name", "My Game Lobby");

        // Join the lobby as the creator
        SteamMatchmaking.JoinLobby(currentLobbyID);

        // Update lobby UI
        lobbyUIManager.UpdateLobbyMembers(currentLobbyID);
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t result)
    {
        SteamMatchmaking.JoinLobby(result.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t result)
    {
        Debug.Log("Successfully entered lobby");
        // Update lobby UI
        lobbyUIManager.UpdateLobbyMembers(currentLobbyID);
    }

    public void InviteFriend()
    {
        // Ensure the Steam overlay is enabled and working
        SteamFriends.ActivateGameOverlayInviteDialog(currentLobbyID);
    }
}
