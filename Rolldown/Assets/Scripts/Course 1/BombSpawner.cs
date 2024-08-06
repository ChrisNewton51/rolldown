using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using System.Linq;

public class BombSpawner : NetworkBehaviour
{
    public GameObject bomb;
    public GameObject bombParent;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (IsServerInitialized)
        {
            this.GiveOwnership(InstanceFinder.ServerManager.Clients.Values.ElementAt(0));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnBomb", 1, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // For bombs obstacle
    [ServerRpc]
    void SpawnBomb()
    {
        foreach (NetworkConnection conn in InstanceFinder.ServerManager.Clients.Values)
        {
            Vector3 localPos = new Vector3(Random.Range(-26, 26), 200, Random.Range(500, 700));
            GameObject newBomb = Instantiate(bomb, transform);
            newBomb.transform.SetParent(bombParent.transform);
            newBomb.transform.localPosition = localPos;
            InstanceFinder.ServerManager.Spawn(newBomb);
        }
        
    }
}
