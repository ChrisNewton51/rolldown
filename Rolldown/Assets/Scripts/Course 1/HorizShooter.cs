using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizShooter : MonoBehaviour
{
    public GameObject smallBomb;
    public GameObject smallBombParent;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnSmallBomb", Random.Range(0,3), Random.Range(1,3));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnSmallBomb()
    {
        GameObject inSmallBomb = Instantiate(smallBomb, transform.position, Quaternion.Euler(15,0,0));
        inSmallBomb.transform.SetParent(smallBombParent.transform);
    }
}
