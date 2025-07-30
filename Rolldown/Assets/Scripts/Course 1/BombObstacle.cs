using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class BombObstacle : NetworkBehaviour
{
    private float ySpeed = 40;
    private float zSpeed = 80;
    public GameObject explosion;
    public GameObject bombParent;

    void Start()
    {
        bombParent = GameObject.Find("Bombs");
    }

    void Update()
    {
        // Ensure only the server runs this logic
        if (!IsServerInitialized) return;

        transform.Translate(new Vector3(0, -Time.deltaTime * ySpeed, -Time.deltaTime * zSpeed));
        if (transform.localPosition.y < -20)
        {
            Despawn();
        }
    }

}
