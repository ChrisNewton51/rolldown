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

    public void FindSpawns()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawn");
        spawns = new Transform[spawners.Length];

        for (int i = 0; i < spawners.Length; i++)
            spawns[i] = spawners[i].transform;
    }

    public void StartGame()
    {
        Destroy(cameraMain);
        FindSpawns();
        foreach (NetworkConnection conn in InstanceFinder.ServerManager.Clients.Values)
        {
            SpawnPlayer(conn, spawns[spawnIndex]);
            spawnIndex++;
        }
        startButton.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        foreach (var player in players)
        {
            player.gameObject.transform.localPosition = new Vector3(0, -6.41f, -266.4171f);
            player.gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            player.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            player.gameObject.transform.rotation = Quaternion.identity;
            player.gameObject.SetActive(false);
        }
    }

    [ServerRpc]
    public void SpawnPlayer(NetworkConnection conn, Transform spawn)
    {
        if (playerPrefab == null)
        {
            Debug.LogWarning($"Player prefab is empty and cannot be spawned for connection {conn.ClientId}.");
            return;
        }

        FindSpawns();

        Vector3 position;
        Quaternion rotation; 

        position = spawns[spawnIndex].position;
        rotation = spawns[spawnIndex].rotation;

        playerPrefab.transform.position = position;
        NetworkObject nob = BootstrapNetworkManager.instance._networkManager.GetPooledInstantiated(playerPrefab, true);
        
        nob.transform.SetPositionAndRotation(position, rotation);
        InstanceFinder.ServerManager.Spawn(nob, conn, UnityEngine.SceneManagement.SceneManager.GetSceneByName("Game"));
        nob.GiveOwnership(conn);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnObject(GameObject obj)
    {
        InstanceFinder.ServerManager.Despawn(obj);
    }
}
