using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombObstacle : MonoBehaviour
{
    private float ySpeed = 40;
    private float zSpeed = 80;
    public GameObject explosion;
    public GameObject bombParent;

    void Start()
    {
        bombParent = GameObject.Find("Bomb Parent");
    }

    void Update()
    {
        transform.Translate(new Vector3(0, -Time.deltaTime * ySpeed, -Time.deltaTime * zSpeed));
        if (transform.localPosition.y < -20)
        {
            Destroy(gameObject);
        }
    }

}
