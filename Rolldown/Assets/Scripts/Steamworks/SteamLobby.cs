using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using TMPro;

public class SteamLobby : MonoBehaviour
{
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<LobbyEnter_t> lobbyEntered;
    protected Callback<LobbyChatUpdate_t> lobbyChatUpdate;

    private CSteamID currentLobbyID;
    private bool hasEnteredLobby = false;

    public GameObject steamLobby;

    private void Awake()
    {
        if (!SteamManager.Initialized) { return; }

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        lobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);
    }

    public void CreateLobby()
    {
        if (SteamManager.Initialized)
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4);
        }
        steamLobby.SetActive(true);
    }

    private void OnLobbyCreated(LobbyCreated_t result)
    {
        if (result.m_eResult != EResult.k_EResultOK)
        {
            Debug.LogError("Failed to create lobby");
            return;
        }

        Debug.Log("Lobby created successfully");
        currentLobbyID = new CSteamID(result.m_ulSteamIDLobby);
        SteamMatchmaking.SetLobbyData(currentLobbyID, "name", "My Game Lobby");

        // Join the lobby as the creator
        SteamMatchmaking.JoinLobby(currentLobbyID);
    }

    private void OnLobbyChatUpdate(LobbyChatUpdate_t result)
    {
        if (result.m_ulSteamIDLobby == (ulong)currentLobbyID)
        {
            Debug.Log("Lobby chat update: " + result.m_ulSteamIDLobby);
        }
    }

    private void OnLobbyEntered(LobbyEnter_t result)
    {
        if (hasEnteredLobby)
        {
            return; // Prevent duplicate calls
        }

        hasEnteredLobby = true;

        Debug.Log("Successfully entered lobby with ID: " + result.m_ulSteamIDLobby);
        currentLobbyID = new CSteamID(result.m_ulSteamIDLobby);
    }

    public void InviteFriend()
    {
        // Ensure the Steam overlay is enabled and working
        SteamFriends.ActivateGameOverlayInviteDialog(currentLobbyID);
    }
}
