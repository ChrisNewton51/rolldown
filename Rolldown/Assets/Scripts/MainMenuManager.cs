using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using TMPro;
using Heathen.SteamworksIntegration;
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

        Heathen.SteamworksIntegration.API.Overlay.Client.OnGameLobbyJoinRequested.AddListener(OverlayJoinButton);

        leaveButton.onClick.AddListener(LeaveLobby);
        hostButton.onClick.AddListener(CreateLobby);
    }

    private void Update()
    {
        if (BootstrapManager.instance.lobbyManager.IsPlayerOwner)
        {
            startButton.gameObject.SetActive(true);
        } else
        {
            startButton.gameObject.SetActive(false);
        }
    }

    public void CreateLobby()
    {
        BootstrapManager.instance.lobbyManager.Create();
    }

    public void OnLobbyCreated(LobbyData lobbyData)
    {
        ClearCards();
        lobbyData.Name = UserData.Me.Name + "'s lobby";
        lobbyTitle.text = UserData.Me.Name + "'s lobby";
        OpenLobby();
        SetupCard(UserData.Me);
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
    }

    public void OpenMainMenu()
    {
        CloseScreens();
        mainMenuObject.SetActive(true);
    }

    public void OtherUserJoin(UserData userData)
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
        
        if (IsSceneLoaded("MainMenu"))
        {
            Destroy(panel.gameObject);
            _lobbyUserPanels.Remove(userLeaveData.user);
        }
    }

    public void OpenLobby()
    {
        CloseScreens();
        lobbyObject.SetActive(true);
    }

    private void OverlayJoinButton(LobbyData lobbyData, UserData user)
    {
        BootstrapManager.instance.lobbyManager.Join(lobbyData);
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
        BootstrapManager.instance.lobbyManager.Leave();
    }

    public void InviteFriend(UserData userData)
    {
        BootstrapManager.instance.lobbyManager.Invite(userData);
    }

    public void StartGame()
    {
        BootstrapSceneManager.instance.StartGame();
    }

    public void ExitGame()
    {
        BootstrapManager.instance.ExitGame();
    }

    public bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            if (scene.name == sceneName && scene.isLoaded)
            {
                return true;
            }
        }
        return false;
    }
}
