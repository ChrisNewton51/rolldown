using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombObstacle : MonoBehaviour
{
    private float ySpeed = 20;
    private float zSpeed = 80;
    public GameObject explosion;
    public GameObject bombParent;

    void Start()
    {
        bombParent = GameObject.Find("Bomb Parent");
    }

    void Update()
    {
        ySpeed *= 1.001f;
        transform.Translate(new Vector3(0, -Time.deltaTime * ySpeed, -Time.deltaTime * zSpeed));
        if (transform.localPosition.y < -20)
        {
            Destroy(gameObject);
            //GameObject exp = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            //exp.transform.SetParent(bombParent.transform);
            //exp.transform.localPosition = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ramp") || other.gameObject.CompareTag("SideR") || other.gameObject.CompareTag("SideL"))
        {
            
        }
    }

}
