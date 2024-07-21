using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Timing;
using FishNet.Object;
using FishNet.Transporting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;

    [Tooltip("Prefab to spawn for the player.")]
    [SerializeField]
    private GameObject playerPrefab;

    public GameObject pauseScreen;
    public GameObject cameraMain;
    public Button startButton;

    private GameObject[] players;
    private Transform[] spawns;
    private int spawnIndex = 0;

    [HideInInspector] public GameObject spawnedObject;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (IsServerInitialized)
        {
            this.GiveOwnership(InstanceFinder.ServerManager.Clients.Values.ElementAt(0));
            startButton.gameObject.SetActive(true);
        }
    }

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        FindSpawns();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
    }

    public void FindSpawns()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawn");
        spawns = new Transform[spawners.Length];

        for (int i = 0; i < spawners.Length; i++)
        {

            spawns[i] = spawners[i].transform;
        }
    }

    public void StartGame()
    {
        Destroy(cameraMain);
        foreach (NetworkConnection conn in InstanceFinder.ServerManager.Clients.Values)
        {
            SpawnPlayer(conn, this);
        }
        startButton.gameObject.SetActive(false);
    }

    public void PauseGame()
    {
        pauseScreen.SetActive(true);
    }

    public void RestartGame()
    {
        foreach (var player in players)
        {
            player.gameObject.transform.localPosition = new Vector3(0, -6.41f, -266.4171f);
            player.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            player.gameObject.transform.rotation = Quaternion.identity;
            player.gameObject.SetActive(false);
        }
        pauseScreen.SetActive(false);
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

    [ServerRpc]
    public void SpawnPlayer(NetworkConnection conn, GameManager script)
    {
        if (playerPrefab == null)
        {
            Debug.LogWarning($"Player prefab is empty and cannot be spawned for connection {conn.ClientId}.");
            return;
        }

        FindSpawns();

        Vector3 position;
        Quaternion rotation; 

        if (spawns.Length > 0)
        {
            Debug.Log(spawns[spawnIndex].localPosition);
            position = spawns[spawnIndex].position;
            rotation = spawns[spawnIndex].rotation;

            spawnIndex++;
            if (spawnIndex >= spawns.Length)
                spawnIndex = 0;
        }
        else
        {
            position = playerPrefab.transform.position;
            rotation = playerPrefab.transform.rotation;
        }

        playerPrefab.transform.position = position;
        NetworkObject nob = BootstrapNetworkManager.instance._networkManager.GetPooledInstantiated(playerPrefab, true);
        
        nob.transform.SetPositionAndRotation(position, rotation);
        InstanceFinder.ServerManager.Spawn(nob, conn, UnityEngine.SceneManagement.SceneManager.GetSceneByName("Game"));
        nob.GiveOwnership(conn);
    }

    //public void SpawnPlayer(GameObject obj, Transform player, GameManager script)
    //{
    //    GameObject spawned = Instantiate(obj, player.position + player.forward, Quaternion.identity);
    //    InstanceFinder.ServerManager.Spawn(spawned);
    //    SetSpawnedPlayer(spawned, script);
    //}

    [ServerRpc(RequireOwnership = false)]
    public void DespawnObject(GameObject obj)
    {
        InstanceFinder.ServerManager.Despawn(obj);
    }
}
