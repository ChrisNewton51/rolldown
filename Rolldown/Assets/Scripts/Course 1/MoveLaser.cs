using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLaser : MonoBehaviour
{
    public float speed = 10;
    private int direction = 1;
    public int backBound = 320;
    public int forwardBound = 396;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * direction * Time.deltaTime);

        if (transform.localPosition.z <= backBound)
        {
            direction = -1;
        }
        if (transform.localPosition.z >= forwardBound)
        {
            direction = 1;
        }
    }
}
