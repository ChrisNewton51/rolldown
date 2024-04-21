using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 40.0f;
    public float distance = 12.0f;

    private PlayerController player;
    private float orbitDamping = 10f;
    private float x = 0.0f;
    private float lookRange = 60f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
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
        Quaternion rotation = Quaternion.Euler(15, 0, 0);
        Vector3 position = rotation * new Vector3(0.0f, 4f, -distance) + player.transform.position;

        if (player.inRArch)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(8.33f, position.y, position.z), Time.deltaTime * orbitDamping);
        } 
        else if (player.inLArch)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(-8.33f, position.y, position.z), Time.deltaTime * orbitDamping);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * orbitDamping);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * orbitDamping);
    }
}
