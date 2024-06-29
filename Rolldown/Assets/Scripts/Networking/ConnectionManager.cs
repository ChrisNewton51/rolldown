using System.Collections;
using System.Collections.Generic;
using HeathenEngineering.SteamworksIntegration;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using FishNet;
using FishNet.Transporting;
using FishNet.Transporting.Tugboat;

public class ConnectionManager : MonoBehaviour
{
    private static ConnectionManager instance;

    [SerializeField] private TMP_InputField connectionInput;
    [SerializeField] private FishySteamworks.FishySteamworks fishySteamworks;

    private string _hostHex;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;

        DontDestroyOnLoad(this);
    }

    

    public void StartHost()
    {
        var user = UserData.Get();
        _hostHex = user.ToString();

        fishySteamworks.StartConnection(true);
        fishySteamworks.StartConnection(false);
        //SceneManager.LoadScene(1);
    }

    public void StartConnection()
    {
        _hostHex = connectionInput.text;
        var hostUser = UserData.Get(_hostHex);

        if (!hostUser.IsValid)
        {
            Debug.LogError("Host user not valid");
            return;
        }

        fishySteamworks.SetClientAddress(hostUser.id.ToString());
        fishySteamworks.StartConnection(false);
        //SceneManager.LoadScene(1);
    }

    public static string GetHostHex()
    {
        return instance._hostHex;
    }
}
