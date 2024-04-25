using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuncherMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.localPosition.x, transform.localPosition.y+5, transform.localPosition.z);
    }
}
