using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public GameObject bomb;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnBomb", 1, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // For bombs obstacle
    void SpawnBomb()
    {
        Instantiate(bomb, new Vector3(Random.Range(-26, 26), 50, Random.Range(1800, 2100)), Quaternion.identity);
    }
}
