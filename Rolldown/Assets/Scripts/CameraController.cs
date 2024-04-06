using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public GameObject floor;
    public float sensitivity = 80.0f;
    public float distance = 8.0f;

    private float orbitDamping = 10f;
    private float x = 0.0f;
    private float lookRange = 60f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LateUpdate()
    {
        CamOrbit();
    }

    private void CamOrbit()
    {
        // Rotate camera around player
        if (Input.GetKey(KeyCode.Mouse1))
        {
            x += Input.GetAxis("Mouse X") * sensitivity * distance * 0.02f;
            
        }
        x = Mathf.Clamp(x, -lookRange, lookRange);
        Quaternion rotation = Quaternion.Euler(20, x, 0);
        Vector3 position = rotation * new Vector3(0.0f, 3.5f, -distance) + player.transform.position;

        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * orbitDamping);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * orbitDamping);
    }
}
