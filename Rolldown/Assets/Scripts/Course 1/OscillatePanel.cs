using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatePanel : MonoBehaviour
{
    private float hSpeed = 4;
    private float vSpeed = 4;
    // Start is called before the first frame update
    void Start()
    {
        hSpeed = Random.Range(4, 6);
        vSpeed = Random.Range(3, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "HOscPanel")
        {
            transform.Translate(Vector3.right * hSpeed * Time.deltaTime);
            if (transform.position.x >= 12.5f || transform.position.x <= 4.35f)
            {
                hSpeed *= -1;
            }
        }

        if (gameObject.name == "VOscPanel")
        {
            transform.Translate(Vector3.down * vSpeed * Time.deltaTime);
            if (transform.localPosition.y >= 3.1f || transform.localPosition.y <= -3.5f)
            {
                vSpeed *= -1;
            }
        }
    }
}
