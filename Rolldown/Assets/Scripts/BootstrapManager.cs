using FishNet.Managing;
using Heathen.SteamworksIntegration;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapManager : MonoBehaviour
{
    public static BootstrapManager instance;

    [SerializeField] private string menuName = "MainMenu";
    [SerializeField] private BootstrapNetworkManager bootstrapNetworkManager;

    [Header("Steam Lobby Defaults")]
    [SerializeField] private int lobbyMaxSlots = 4;
    [SerializeField] private SteamLobbyCreate.SteamLobbyType lobbyType = SteamLobbyCreate.SteamLobbyType.FriendsOnly;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        EnsureSteamLobby();
        SteamTools.Interface.WhenReady(GoToMenu);
    }

    private void EnsureSteamLobby()
    {
        if (bootstrapNetworkManager == null)
            bootstrapNetworkManager = FindFirstObjectByType<BootstrapNetworkManager>();

        var existingLobby = FindFirstObjectByType<SteamLobby>();
        if (existingLobby != null)
        {
            DontDestroyOnLoad(existingLobby.gameObject);
            existingLobby.Configure(bootstrapNetworkManager, lobbyMaxSlots, lobbyType);
            return;
        }

        // Keep inactive until SteamLobby.Configure finishes adding Heathen components.
        // Runtime AddComponent does not initialize UnityEvents, and SteamLobbyDataEvents.Awake
        // requires them to exist before the GameObject is activated.
        var lobbyObject = new GameObject("SteamLobby");
        lobbyObject.SetActive(false);
        DontDestroyOnLoad(lobbyObject);

        var steamLobby = lobbyObject.AddComponent<SteamLobby>();
        steamLobby.Configure(bootstrapNetworkManager, lobbyMaxSlots, lobbyType);
        lobbyObject.SetActive(true);
    }

    public void GoToMenu()
    {
        if (MainMenuManager.instance != null && MainMenuManager.instance.IsSceneLoaded(menuName))
            return;

        SceneManager.LoadScene(menuName, LoadSceneMode.Additive);
    }

    public void ExitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
