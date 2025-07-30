using FishNet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using System.Linq;

public class HorizShooter : NetworkBehaviour
{
    public NetworkObject smallBomb;
    public Transform smallBombParent;

    public override void OnStartServer()
    {
        base.OnStartServer();

        InvokeRepeating("SpawnSmallBomb", Random.Range(0, 3), Random.Range(1, 3));
    }

    void SpawnSmallBomb()
    {
        NetworkObject inSmallBomb = Instantiate(smallBomb, transform.position, Quaternion.Euler(15, 0, 0));
        inSmallBomb.transform.SetParent(smallBombParent);
        InstanceFinder.ServerManager.Spawn(inSmallBomb);
    }
}
