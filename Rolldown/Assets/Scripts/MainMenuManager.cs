using System.Collections.Generic;
using FishNet;
using Heathen.SteamworksIntegration;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    private readonly Dictionary<UserData, LobbyUserPanel> _lobbyUserPanels = new();

    private void Awake()
    {
        instance = this;
        OpenMainMenu();
    }

    private void Start()
    {
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
        lobbyData["name"] = $"{UserData.Me.Name}'s Lobby";
        OnLobbyJoined(lobbyData);
    }

    public void OnLobbyJoined(LobbyData lobbyData)
    {
        mainMenuObject.SetActive(false);
        lobbyObject.SetActive(true);
        lobbyTitle.text = lobbyData["name"];
        RefreshPlayerList(lobbyData);
    }

    #endregion

    #region Button Actions

    public void CreateLobby()
    {
        if (SteamLobby.Instance == null)
        {
            Debug.LogError("SteamLobby is not initialized. Ensure the Bootstrap scene runs first.");
            return;
        }

        SteamLobby.Instance.CreateLobby();
    }

    public void LeaveLobby()
    {
        SteamLobby.Instance?.LeaveLobby();
        OpenMainMenu();
    }

    public void StartGame()
    {
        BootstrapSceneManager.instance.StartGame();
    }

    public void ExitGame()
    {
        BootstrapManager.instance.ExitGame();
    }

    #endregion

    #region Internal Logic & UI

    private void RefreshPlayerList(LobbyData lobby)
    {
        ClearCards();

        foreach (var member in lobby.Members)
            SetupCard(member.user);
    }

    public void SetupCard(UserData userData)
    {
        if (_lobbyUserPanels.ContainsKey(userData))
            return;

        var userPanel = Instantiate(lobbyUserPanelPrefab, lobbyUserHolder);
        userPanel.Initialize(userData);
        _lobbyUserPanels.Add(userData, userPanel);
    }

    public void RemoveCard(UserData userData)
    {
        if (!_lobbyUserPanels.TryGetValue(userData, out var panel))
            return;

        Destroy(panel.gameObject);
        _lobbyUserPanels.Remove(userData);
    }

    private void ClearCards()
    {
        foreach (var panel in _lobbyUserPanels.Values)
            Destroy(panel.gameObject);

        _lobbyUserPanels.Clear();
    }

    public bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == sceneName)
                return true;
        }

        return false;
    }

    public void OpenInviteOverlay()
    {
        SteamLobby.Instance?.OpenInviteOverlay();
    }

    #endregion
}
