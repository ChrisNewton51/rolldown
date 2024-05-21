using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class OscillatePanel : MonoBehaviour
{
    private float hSpeed = 4;
    private float vSpeed = 4;
    private int hDirection = 1;
    private int vDirection = 1;
    // Start is called before the first frame update
    void Start()
    {
        hSpeed = Random.Range(4, 6);
        vSpeed = Random.Range(2, 4);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "HOscPanel")
        {
            transform.Translate(Vector3.right * hSpeed * hDirection * Time.deltaTime);
            if (transform.position.x >= 12.5f)
            {
                hDirection = -1;
            } else if (transform.position.x <= 4.35f)
            {
                hDirection = 1;
            }
        }

        if (gameObject.name == "VOscPanel")
        {
            transform.Translate(Vector3.down * vSpeed * vDirection * Time.deltaTime);
            if (transform.localPosition.y >= 3.1f)
            {
                vDirection = 1;
            } else if (transform.localPosition.y <= -3.5f)
            {
                vDirection = -1;
            }
        }
    }
}
