using FishNet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using System.Linq;

public class HorizShooter : NetworkBehaviour
{
    public GameObject smallBomb;
    public GameObject smallBombParent;

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
        InvokeRepeating("SpawnSmallBomb", Random.Range(0,3), Random.Range(1,3));
    }

    [ServerRpc]
    void SpawnSmallBomb()
    {
        GameObject inSmallBomb = Instantiate(smallBomb, transform.position, Quaternion.Euler(15, 0, 0));
        inSmallBomb.transform.SetParent(smallBombParent.transform);
        InstanceFinder.ServerManager.Spawn(inSmallBomb);
    }
}
