using FishNet.Object;
using FishNet;
using UnityEngine;

public class BombSpawner : NetworkBehaviour
{
    [SerializeField] private NetworkObject bombPrefab;      // Must be a prefab with NetworkObject
    [SerializeField] private Transform bombParent;          // Optional parent for organization

    // Called only on the server when the object spawns
    public override void OnStartServer()
    {
        base.OnStartServer();

        // Begin spawning bombs every 0.2s, starting after 1s
        InvokeRepeating(nameof(SpawnBomb), 1f, 0.2f);
    }

    // Mark as server-only to prevent clients from calling it
    [Server]
    private void SpawnBomb()
    {
        Vector3 basePos = bombParent.position;

        // 1. Compute the exact world position
        Vector3 spawnPos = basePos + new Vector3(Random.Range(-26f, 26f), 200f, Random.Range(500f, 700f));

        // 2. Instantiate with NULL parent to ensure spawnPos is treated as absolute World Space
        NetworkObject instance = Instantiate(
            bombPrefab,
            spawnPos,
            Quaternion.identity,
            null // Set parent to null initially
        );

        // 3. Spawn it to the network
        InstanceFinder.ServerManager.Spawn(instance);

        // 4. (Optional) Re-parent for organization on the server
        // Note: Only do this if bombParent also has a NetworkObject. 
        // If it doesn't, it's safer to leave them unparented.
        instance.transform.SetParent(bombParent);
    }
}
