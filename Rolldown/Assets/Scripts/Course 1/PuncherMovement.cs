using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuncherMovement : MonoBehaviour
{
    public float topLimit = -5f;

    private float bottomLimit = -15f;
    private float punchSpeed = 8;
    private float upSpeed = 8;
    private float downSpeed = -4;

    void Start()
    {
        bottomLimit = Random.Range(-15, -11);    
        transform.localPosition = new Vector3(transform.localPosition.x, bottomLimit + 1, transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * punchSpeed);
        
        if (transform.localPosition.y >= topLimit)
        {
            punchSpeed = downSpeed;
            gameObject.GetComponent<Collider>().isTrigger = false;
            gameObject.GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);
            gameObject.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);
            gameObject.tag = "Floor";
        }
        if (transform.localPosition.y <= bottomLimit) 
        {
            punchSpeed = upSpeed;
            gameObject.GetComponent<Collider>().isTrigger = true;
            gameObject.GetComponent<BoxCollider>().size = new Vector3(1, 0, 1);
            gameObject.GetComponent<BoxCollider>().center = new Vector3(0, .5f, 0);
            gameObject.tag = "Puncher";
        }
    }
}
