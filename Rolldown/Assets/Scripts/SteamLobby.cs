using UnityEngine;
using Steamworks;
using Heathen.SteamworksIntegration; // Your preferred namespace

public class SteamLobby : MonoBehaviour
{   
    public static SteamLobby Instance;

    [SerializeField] private LobbyManager lobbyManager;
    [SerializeField] private BootstrapNetworkManager bootstrapNetworkManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Linked to LobbyManager.evtCreated
    public void OnLobbyCreated(LobbyData lobbyData)
    {
        MainMenuManager.instance.OnLobbyCreated(lobbyData);

        // Use UserData.Me for the local user and indexers for metadata
        string hostId = UserData.Me.SteamId.ToString();
        lobbyData["HostID"] = hostId;
        
        bootstrapNetworkManager.LobbyCreated(hostId);
    }

    // Linked to LobbyManager.evtEnterSuccess
    public void OnLobbyJoined(LobbyData lobbyData) 
    {
        MainMenuManager.instance.OnLobbyJoined(lobbyData);

        // Get metadata using the indexer
        string hostId = lobbyData["HostID"];
        bootstrapNetworkManager.LobbyJoined(hostId);
    }

    // Linked to LobbyManager.evtUserJoined (passes UserData)
    public void OtherUserJoin(UserData userData)
    {
        MainMenuManager.instance.SetupCard(userData);
    }

    // Linked to LobbyManager.evtUserLeft (passes UserLobbyLeaveData)
    public void OnUserLeft(UserLobbyLeaveData userLeaveData)
    {
        // UserLobbyLeaveData contains the UserData object
        MainMenuManager.instance.RemoveCard(userLeaveData.user);
    }

    public void LobbyLeave()
    {
        BootstrapNetworkManager.instance.LeaveLobby();
    }
}