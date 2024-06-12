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

    private void Awake()
    {
        if (!SteamManager.Initialized) { return; }

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public static void CreateLobby()
    {
        if (SteamManager.Initialized)
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4);
        }
    }

    private static void OnLobbyCreated(LobbyCreated_t result)
    {
        if (result.m_eResult != EResult.k_EResultOK)
        {
            Debug.Log("Failed to create lobby");
            return;
        }

        Debug.Log("Lobby created successfully");
        CSteamID lobbyId = new CSteamID(result.m_ulSteamIDLobby);
        SteamMatchmaking.SetLobbyData(lobbyId, "name", "My Game Lobby");
    }

    private static void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t result)
    {
        SteamMatchmaking.JoinLobby(result.m_steamIDLobby);
    }

    private static void OnLobbyEntered(LobbyEnter_t result)
    {
        Debug.Log("Successfully entered lobby");
        // Additional code to handle the player entering the lobby
    }

    public static void InviteFriend()
    {
        // Ensure the Steam overlay is enabled and working
        SteamFriends.ActivateGameOverlayInviteDialog(SteamUser.GetSteamID());
    }
}
