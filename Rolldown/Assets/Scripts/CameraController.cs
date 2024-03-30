using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0,6,-6);

    // Start is called before the first frame update
    void Start()
    {

    }

    void LateUpdate()
    {
        // Follow player
        transform.position = player.transform.position + offset;
    }
}
