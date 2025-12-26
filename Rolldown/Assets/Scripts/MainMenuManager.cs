using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using TMPro;
using Heathen.SteamworksIntegration; // Updated Namespace
using Heathen.SteamworksIntegration.API; // For Overlay
using UnityEngine.UI;
using FishNet.Managing.Scened;
using FishNet.Managing;
using FishNet.Connection;
using UnityEngine.SceneManagement;
using FishNet.Object;
using FishNet;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    [Header("Main")]
    [SerializeField] private GameObject mainMenuObject;

    [Header("Lobby")]
    [SerializeField] private Button hostButton;
    [SerializeField] private GameObject lobbyObject;
    [SerializeField] private TextMeshProUGUI lobbyTitle;
    [SerializeField] private Button leaveButton;
    [SerializeField] private Button startButton;

    [Header("User lobby setup")]
    [SerializeField] private LobbyUserPanel lobbyUserPanelPrefab;
    [SerializeField] private Transform lobbyUserHolder;

    [SerializeField] private GameObject mainCamera;

    private Dictionary<UserData, LobbyUserPanel> _lobbyUserPanels = new();

    private void Awake()
    {
        instance = this;
        OpenMainMenu();
    }

    private void Start()
    {
        // Subscribe to the Overlay join requested event using the 2026 API
        Overlay.Client.OnGameLobbyJoinRequested.AddListener(OnOverlayJoinRequested);

        // UI Button setup
        leaveButton.onClick.AddListener(LeaveLobby);
        hostButton.onClick.AddListener(CreateLobby);
    }

    public void OpenMainMenu()
    {
        mainMenuObject.SetActive(true);
        lobbyObject.SetActive(false);
        ClearCards();
    }

    #region SteamLobby Callbacks
    public void OnLobbyCreated(LobbyData lobbyData)
    {
        // 2026 syntax: use indexers for metadata
        lobbyData["name"] = $"{UserData.Me.Name}'s Lobby";
        OnLobbyJoined(lobbyData);
    }

    public void OnLobbyJoined(LobbyData lobbyData)
    {
        mainMenuObject.SetActive(false);
        lobbyObject.SetActive(true);
        
        // Retrieve metadata using indexer
        lobbyTitle.text = lobbyData["name"];
        
        RefreshPlayerList(lobbyData);
    }

    public void OtherUserJoin(UserData userData) => SetupCard(userData);

    // Handles the new UserLobbyLeaveData struct from the 2026 events
    public void OnUserLeft(UserLobbyLeaveData userLeaveData) => RemoveCard(userLeaveData.user);
    #endregion

    #region Button Actions
    public void CreateLobby()
    {
        // Finds the manager in the Bootstrap scene
        var manager = FindFirstObjectByType<LobbyManager>();
        manager?.Create(); 
    }

    public void LeaveLobby()
    {
        var manager = FindFirstObjectByType<LobbyManager>();
        manager?.Leave();
        OpenMainMenu();
    }

    public void StartGame()
    {
        // This triggers the FishNet scene load logic
        BootstrapSceneManager.instance.StartGame();
    }

    public void ExitGame()
    {
        // Triggers the quit logic in your Bootstrap manager
        BootstrapManager.instance.ExitGame();
    }
    #endregion

    #region Internal Logic & UI
    private void OnOverlayJoinRequested(LobbyData lobby, UserData user)
    {
        var manager = FindFirstObjectByType<LobbyManager>();
        manager?.Join(lobby);
    }

    private void RefreshPlayerList(LobbyData lobby)
    {
        ClearCards();
        // Uses the Members property from the new LobbyData struct
        foreach (var member in lobby.Members)
        {
            SetupCard(member.user);
        }
    }

    public void SetupCard(UserData userData)
    {
        if (!_lobbyUserPanels.ContainsKey(userData))
        {
            var userPanel = Instantiate(lobbyUserPanelPrefab, lobbyUserHolder);
            userPanel.Initialize(userData);
            _lobbyUserPanels.Add(userData, userPanel);
        }
    }

    public void RemoveCard(UserData userData)
    {
        if (_lobbyUserPanels.TryGetValue(userData, out var panel))
        {
            Destroy(panel.gameObject);
            _lobbyUserPanels.Remove(userData);
        }
    }

    private void ClearCards()
    {
        foreach (var panel in _lobbyUserPanels.Values) Destroy(panel.gameObject);
        _lobbyUserPanels.Clear();
    }

    public bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            if (scene.name == sceneName) return true;
        }
        return false;
    }

    public void OpenInviteOverlay()
    {
        var manager = FindFirstObjectByType<LobbyManager>();
        if (manager != null && manager.HasLobby)
        {
            // Use the 2026 Overlay API to open the invite dialog for the current lobby
            Overlay.Client.ActivateInviteDialog(manager.Data);
        }
    }
    #endregion
}