using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using TMPro;
using HeathenEngineering.SteamworksIntegration;


public class LobbyUIManager : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private GameObject mainMenuObject;

    [Header("Lobby")]
    [SerializeField] private GameObject lobbyObject;
    [SerializeField] private TextMeshProUGUI lobbyTitle;
    [SerializeField] private LobbyManager lobbyManager;

    private void Awake()
    {
        OpenMainMenu();
        HeathenEngineering.SteamworksIntegration.API.Overlay.Client.EventGameLobbyJoinRequested.AddListener(OverlayJoinButton);
    }

    public void OnLobbyCreated(LobbyData lobbyData)
    {
        lobbyData.Name = UserData.Me.Name + "'s lobby";
        lobbyTitle.text = UserData.Me.Name + "'s lobby";
        OpenLobby();
    }

    public void OnLobbyJoined(LobbyData lobbyData)
    {
        lobbyTitle.text = lobbyData.Name;
        OpenLobby();
    }

    public void OpenMainMenu()
    {
        CloseScreens();
        mainMenuObject.SetActive(true);
    }

    public void OpenLobby()
    {
        CloseScreens();
        lobbyObject.SetActive(true);
    }

    private void OverlayJoinButton(LobbyData lobbyData, UserData user)
    {
        lobbyManager.Join(lobbyData);
    }

    private void CloseScreens()
    {
        mainMenuObject.SetActive(false);
        lobbyObject.SetActive(false);
    }
}
