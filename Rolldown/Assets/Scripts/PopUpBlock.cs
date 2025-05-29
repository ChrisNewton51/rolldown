using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class PopUpBlock : MonoBehaviour
{
    private float moveSpeed;
    private float movement;
    private int lowerBound = -15;
    private int upperBound = -5;
    private float tempTime;
    private float delay = 2f;
    private bool switchDir = false;
   
    void Start()
    {
        tempTime = Time.time;
        InvokeRepeating("ShootUp", 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(ShootUp());
        movement = Time.deltaTime * moveSpeed;
        if (transform.localPosition.y >= upperBound)
        {
            moveSpeed = -1;
        }
        if (transform.localPosition.y <= lowerBound)
        {
            moveSpeed = 20f;
        }
        transform.Translate(0, movement, 0);

        //tempTime += Time.deltaTime;
        //if (tempTime > delay)
        //{
        //    switchDir = true;
        //    tempTime = 0;
        //}
        //if (switchDir)
        //{
        //    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), Time.deltaTime * 10);
        //    switchDir = false;
        //}
    }

    private void ShootUp()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), Time.deltaTime * (Time.timeScale / 2));
        

            //transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -10, transform.position.z), Time.deltaTime);

    }
}
