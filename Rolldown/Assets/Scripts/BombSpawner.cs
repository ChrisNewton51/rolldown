using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public GameObject bomb;
    public GameObject bombParent;

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
    void SpawnBomb()
    {
        Vector3 localPos = new Vector3(Random.Range(-26, 26), 200, Random.Range(400, 800));
        GameObject newBomb = Instantiate(bomb, transform);
        newBomb.transform.SetParent(bombParent.transform);
        newBomb.transform.localPosition = localPos;
    }
}
