using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using TMPro;
using HeathenEngineering.SteamworksIntegration;
using UnityEngine.UI;
using FishNet.Managing.Scened;
using FishNet.Managing;
using FishNet.Connection;


public class MainMenuManager : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private GameObject mainMenuObject;

    [Header("Lobby")]
    [SerializeField] private GameObject lobbyObject;
    [SerializeField] private TextMeshProUGUI lobbyTitle;
    [SerializeField] private LobbyManager lobbyManager;
    [SerializeField] private Button leaveButton;

    [Header("User lobby setup")]
    [SerializeField] private LobbyUserPanel lobbyUserPanelPrefab;
    [SerializeField] private Transform lobbyUserHolder;

    [SerializeField] private GameObject mainCamera;

    private Dictionary<UserData, LobbyUserPanel> _lobbyUserPanels = new();

    private void Awake()
    {
        OpenMainMenu();
        HeathenEngineering.SteamworksIntegration.API.Overlay.Client.EventGameLobbyJoinRequested.AddListener(OverlayJoinButton);
        leaveButton.onClick.AddListener(LeaveLobby);
    }

    public void OnLobbyCreated(LobbyData lobbyData)
    {
        ClearCards();
        lobbyData.Name = UserData.Me.Name + "'s lobby";
        lobbyTitle.text = UserData.Me.Name + "'s lobby";
        OpenLobby();

        string hostId = UserData.Get().ToString();
        lobbyManager.SetLobbyData("HostID", hostId);

        SetupCard(UserData.Me);
        BootstrapNetworkManager.instance.LobbyCreated();
    }

    public void OnLobbyJoined(LobbyData lobbyData)
    {
        ClearCards();
        lobbyTitle.text = lobbyData.Name;
        OpenLobby();

        foreach (var member in lobbyData.Members)
        {
            SetupCard(member.user);
        }
        
        string hostId = lobbyManager.GetLobbyData("HostID");
        BootstrapNetworkManager.instance.LobbyJoined(hostId);
    }

    public void OpenMainMenu()
    {
        CloseScreens();
        mainMenuObject.SetActive(true);
    }

    public void OnUserJoin(UserData userData)
    {
        SetupCard(userData);
    }

    public void OnUserLeft(UserLobbyLeaveData userLeaveData)
    {
        if(!_lobbyUserPanels.TryGetValue(userLeaveData.user, out LobbyUserPanel panel))
        {
            Debug.LogError("Tried to remove user that doesn't exist");
            return;
        }

        Destroy(panel.gameObject);
        _lobbyUserPanels.Remove(userLeaveData.user);
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

    private void ClearCards()
    {
        foreach (Transform child in lobbyUserHolder)
            Destroy(child.gameObject);

        _lobbyUserPanels.Clear();
    }

    private void SetupCard(UserData userData)
    {
        var userPanel = Instantiate(lobbyUserPanelPrefab, lobbyUserHolder);
        userPanel.Initialize(userData);

        _lobbyUserPanels.TryAdd(userData, userPanel);
    }

    public void LeaveLobby()
    {
        lobbyManager.Leave();
        BootstrapNetworkManager.instance.LeaveLobby();
    }

    public void StartGame()
    {
        mainCamera.SetActive(false);
        string[] scenesToClose = new string[] { "MainMenu" };
        BootstrapSceneManager.instance.StartGame();
    }

    public void ListConnections()
    {
        BootstrapNetworkManager.instance.ListConnectedClients();
    }
}
