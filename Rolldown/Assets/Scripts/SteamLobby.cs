using Heathen.SteamworksIntegration;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-1000)]
public class SteamLobby : MonoBehaviour
{
    public static SteamLobby Instance { get; private set; }

    private SteamLobbyData _lobbyData;
    private SteamLobbyDataEvents _lobbyEvents;
    private SteamLobbyCreate _lobbyCreate;
    private SteamLobbyJoin _lobbyJoin;
    private SteamLobbyLeave _lobbyLeave;

    private BootstrapNetworkManager _bootstrapNetworkManager;
    private bool _isConfigured;

    public bool HasLobby => _lobbyData != null && _lobbyData.Data.IsValid;
    public LobbyData CurrentLobby => _lobbyData != null ? _lobbyData.Data : default;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Scene-placed Heathen components can Awake before Configure; heal null UnityEvents first.
        var existingData = GetComponent<SteamLobbyData>();
        if (existingData != null && existingData.onChanged == null)
            existingData.onChanged = new LobbyDataEvent();

        var existingEvents = GetComponent<SteamLobbyDataEvents>();
        if (existingEvents != null)
            InitializeRuntimeEvents(existingEvents);
    }

    public void Configure(
        BootstrapNetworkManager bootstrapNetworkManager,
        int lobbyMaxSlots,
        SteamLobbyCreate.SteamLobbyType lobbyType)
    {
        if (_isConfigured)
            return;

        _bootstrapNetworkManager = bootstrapNetworkManager;

        bool wasActive = gameObject.activeSelf;
        if (wasActive)
            gameObject.SetActive(false);

        EnsureComponents();

        if (_lobbyCreate != null)
        {
            _lobbyCreate.slots = lobbyMaxSlots;
            _lobbyCreate.type = lobbyType;
            _lobbyCreate.usageHint = SteamLobbyModeType.Session;
        }

        if (wasActive)
            gameObject.SetActive(true);

        SubscribeToEvents();
        _isConfigured = true;
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();

        if (Instance == this)
            Instance = null;
    }

    private void EnsureComponents()
    {
        EnsureSteamLobbyData();
        EnsureSteamLobbyDataEvents();
        EnsureOptionalComponents();
    }

    private void EnsureSteamLobbyData()
    {
        _lobbyData ??= GetComponent<SteamLobbyData>();
        if (_lobbyData == null)
            _lobbyData = gameObject.AddComponent<SteamLobbyData>();

        if (_lobbyData.onChanged == null)
            _lobbyData.onChanged = new LobbyDataEvent();
    }

    private void EnsureSteamLobbyDataEvents()
    {
        _lobbyEvents = GetComponent<SteamLobbyDataEvents>();

        // Runtime AddComponent leaves UnityEvents null and can leave Awake in a broken state.
        if (_lobbyEvents != null && _lobbyEvents.onCreate == null)
        {
            Destroy(_lobbyEvents);
            _lobbyEvents = null;
        }

        if (_lobbyEvents == null)
            _lobbyEvents = gameObject.AddComponent<SteamLobbyDataEvents>();

        InitializeRuntimeEvents(_lobbyEvents);
    }

    private void EnsureOptionalComponents()
    {
        _lobbyCreate ??= GetComponent<SteamLobbyCreate>();
        _lobbyJoin ??= GetComponent<SteamLobbyJoin>();
        _lobbyLeave ??= GetComponent<SteamLobbyLeave>();

        if (_lobbyCreate == null)
            _lobbyCreate = gameObject.AddComponent<SteamLobbyCreate>();
        if (_lobbyJoin == null)
            _lobbyJoin = gameObject.AddComponent<SteamLobbyJoin>();
        if (_lobbyLeave == null)
            _lobbyLeave = gameObject.AddComponent<SteamLobbyLeave>();

        if (GetComponent<SteamLobbyJoinOnInvite>() == null)
        {
            var joinOnInvite = gameObject.AddComponent<SteamLobbyJoinOnInvite>();
            joinOnInvite.mode = SteamLobbyJoinOnInvite.JoinOnMode.AfterAcceptInFriendChat;
        }
    }

    private static void InitializeRuntimeEvents(SteamLobbyDataEvents events)
    {
        if (events == null)
            return;

        events.onLobbyChange ??= new LobbyDataEvent();
        events.onLobbySet ??= new UnityEvent<bool>();
        events.onLobbyRemoved ??= new UnityEvent<bool>();
        events.onLobbyIdChanged ??= new UnityEvent<string>();
        events.onLobbySetIsOwner ??= new UnityEvent<bool>();
        events.onLobbySetIsNotOwner ??= new UnityEvent<bool>();
        events.onLobbySetIsMember ??= new UnityEvent<bool>();
        events.onLobbySetIsNotMember ??= new UnityEvent<bool>();
        events.onLobbyInvite ??= new UnityEvent<UserData, LobbyData, GameData>();
        events.onLobbyJoinRequest ??= new GameLobbyJoinRequestedEvent();
        events.onChatMessage ??= new LobbyChatMsgEvent();
        events.onSearchResult ??= new LobbyDataListEvent();
        events.onEnterSuccess ??= new LobbyDataEvent();
        events.onEnterFailure ??= new LobbyResponseEvent();
        events.onCreate ??= new LobbyDataEvent();
        events.onCreationFailure ??= new EResultEvent();
        events.onQuickMatchFailure ??= new UnityEvent();
        events.onDataUpdate ??= new UnityEvent<LobbyData, LobbyMemberData?>();
        events.onUserLeft ??= new UnityEvent();
        events.onAskedToLeave ??= new UnityEvent();
        events.onGameCreate ??= new GameServerSetEvent();
        events.onOtherUserJoined ??= new UserDataEvent();
        events.onOtherUserLeft ??= new UserLeaveEvent();
        events.onAuthenticationSessionResult ??= new LobbyAuthenticaitonSessionEvent();
    }

    private void SubscribeToEvents()
    {
        if (_lobbyEvents == null)
            return;

        InitializeRuntimeEvents(_lobbyEvents);

        _lobbyEvents.onCreate.RemoveListener(HandleLobbyCreated);
        _lobbyEvents.onEnterSuccess.RemoveListener(HandleLobbyJoined);
        _lobbyEvents.onEnterFailure.RemoveListener(HandleLobbyEnterFailed);
        _lobbyEvents.onUserLeft.RemoveListener(HandleLobbyLeave);
        _lobbyEvents.onOtherUserJoined.RemoveListener(HandleOtherUserJoined);
        _lobbyEvents.onOtherUserLeft.RemoveListener(HandleOtherUserLeft);
        _lobbyEvents.onChatMessage.RemoveListener(HandleChatMessage);

        _lobbyEvents.onCreate.AddListener(HandleLobbyCreated);
        _lobbyEvents.onEnterSuccess.AddListener(HandleLobbyJoined);
        _lobbyEvents.onEnterFailure.AddListener(HandleLobbyEnterFailed);
        _lobbyEvents.onUserLeft.AddListener(HandleLobbyLeave);
        _lobbyEvents.onOtherUserJoined.AddListener(HandleOtherUserJoined);
        _lobbyEvents.onOtherUserLeft.AddListener(HandleOtherUserLeft);
        _lobbyEvents.onChatMessage.AddListener(HandleChatMessage);
    }

    private void UnsubscribeFromEvents()
    {
        if (_lobbyEvents == null)
            return;

        _lobbyEvents.onCreate.RemoveListener(HandleLobbyCreated);
        _lobbyEvents.onEnterSuccess.RemoveListener(HandleLobbyJoined);
        _lobbyEvents.onEnterFailure.RemoveListener(HandleLobbyEnterFailed);
        _lobbyEvents.onUserLeft.RemoveListener(HandleLobbyLeave);
        _lobbyEvents.onOtherUserJoined.RemoveListener(HandleOtherUserJoined);
        _lobbyEvents.onOtherUserLeft.RemoveListener(HandleOtherUserLeft);
        _lobbyEvents.onChatMessage.RemoveListener(HandleChatMessage);
    }

    public void CreateLobby()
    {
        _lobbyCreate?.Create();
    }

    public void JoinLobby(LobbyData lobby)
    {
        _lobbyJoin?.Join(lobby);
    }

    public void LeaveLobby()
    {
        _lobbyLeave?.Leave();
    }

    public void OpenInviteOverlay()
    {
        if (!HasLobby)
            return;

        Heathen.SteamworksIntegration.API.Overlay.Client.ActivateInviteDialog(CurrentLobby);
    }

    private void HandleLobbyCreated(LobbyData lobbyData)
    {
        lobbyData.SetGameServer();

        string hostId = UserData.Me.SteamId.ToString();
        lobbyData["HostID"] = hostId;

        MainMenuManager.instance?.OnLobbyCreated(lobbyData);
        _bootstrapNetworkManager?.LobbyCreated(hostId);
    }

    private void HandleLobbyJoined(LobbyData lobbyData)
    {
        MainMenuManager.instance?.OnLobbyJoined(lobbyData);

        string hostId = lobbyData["HostID"];
        if (!string.IsNullOrEmpty(hostId))
            _bootstrapNetworkManager?.LobbyJoined(hostId);
    }

    private void HandleLobbyEnterFailed(EChatRoomEnterResponse response)
    {
        Debug.LogWarning($"Failed to enter Steam lobby: {response}");
        MainMenuManager.instance?.OpenMainMenu();
    }

    private void HandleLobbyLeave()
    {
        _bootstrapNetworkManager?.LeaveLobby();
        MainMenuManager.instance?.OpenMainMenu();
    }

    private void HandleOtherUserJoined(UserData userData)
    {
        MainMenuManager.instance?.SetupCard(userData);
    }

    private void HandleOtherUserLeft(UserLobbyLeaveData userLeaveData)
    {
        MainMenuManager.instance?.RemoveCard(userLeaveData.user);
    }

    private void HandleChatMessage(LobbyChatMsg message)
    {
        Debug.Log($"Lobby chat from {message.sender.Name}: {message.Message}");
    }
}
