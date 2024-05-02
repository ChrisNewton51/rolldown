using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuncherMovement : MonoBehaviour
{
    public float bottomLimit = -15f;
    public float topLimit = -5f;

    private float punchSpeed = 8;
    private float upSpeed = 8;
    private float downSpeed = -2;

    void Start()
    {
        // Randomize position and bottom limit    
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * punchSpeed);
        Debug.Log(gameObject.GetComponent<BoxCollider>().center);
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
