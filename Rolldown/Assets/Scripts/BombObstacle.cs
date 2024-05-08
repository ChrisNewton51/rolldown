using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombObstacle : MonoBehaviour
{
    public float speed = 5;
    

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(new Vector3(0, -Time.deltaTime * speed, -Time.deltaTime * speed));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ramp") || other.gameObject.CompareTag("SideR") || other.gameObject.CompareTag("SideL"))
        {
            Destroy(gameObject);
        }
    }

}
