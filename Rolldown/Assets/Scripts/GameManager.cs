using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Transporting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;

    [Tooltip("Prefab to spawn for the player.")]
    [SerializeField]
    private GameObject playerPrefab;

    public GameObject pauseScreen;
    public GameObject cameraMain;

    private GameObject[] players;
    private Transform[] spawns;
    private int spawnIndex = 0;

    [HideInInspector] public GameObject spawnedObject;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (IsServerInitialized) 
            this.GiveOwnership(BootstrapSceneManager.instance.serverClientConnection);
    }

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        FindSpawns();
        //SpawnPlayer(BootstrapSceneManager.instance.serverClientConnection, this);
        //SpawnPlayer(playerPrefab, spawns[0].transform, this);
        
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

    

    public void PauseGame()
    {
        //pauseScreen.SetActive(true);
        cameraMain.SetActive(false);
        SpawnPlayer(BootstrapSceneManager.instance.serverClientConnection, this);
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
        //Debug.Log($"Received on the server.");
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

        Debug.Log($"Position: {position}");
        Debug.Log($"Rotation: {rotation}");

        playerPrefab.transform.position = position;
        NetworkObject nob = BootstrapNetworkManager.instance._networkManager.GetPooledInstantiated(playerPrefab, true);
        nob.transform.SetPositionAndRotation(position, rotation);
        InstanceFinder.ServerManager.Spawn(nob, conn, UnityEngine.SceneManagement.SceneManager.GetSceneByName("Game"));
        nob.GiveOwnership(conn);
        //SetSpawnedPlayer(nob, script);
    }

    //public void SpawnPlayer(GameObject obj, Transform player, GameManager script)
    //{
    //    GameObject spawned = Instantiate(obj, player.position + player.forward, Quaternion.identity);
    //    InstanceFinder.ServerManager.Spawn(spawned);
    //    SetSpawnedPlayer(spawned, script);
    //}

    //[ObserversRpc]
    public void SetSpawnedPlayer(GameObject spawned, GameManager script)
    {
        script.spawnedObject = spawned;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnObject(GameObject obj)
    {
        ServerManager.Despawn(obj);
    }
}
