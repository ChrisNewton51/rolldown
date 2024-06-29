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
    public PlayerController player;

    private float orbitDamping = 10f;
    private float x = 0.0f;
    private float lookRange = 60f;
    private float rvZ = 19.3f;
    private float rvY = 5.2f;
    private Camera thisCamera;
    private GameObject camObject;

    // Start is called before the first frame update
    void Start()
    {
        thisCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
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

        if (Input.GetKey(KeyCode.Tab))
        {
            orbitDamping = 100;
            if (player.inRArch)
            {
                thisCamera.nearClipPlane = 2;
                transform.position = Vector3.Lerp(transform.position, new Vector3(8.33f, position.y - rvY, position.z + rvZ), Time.deltaTime * orbitDamping);
            }
            else if (player.inLArch)
            {
                thisCamera.nearClipPlane = 2;
                transform.position = Vector3.Lerp(transform.position, new Vector3(-8.33f, position.y - rvY, position.z + rvZ), Time.deltaTime * orbitDamping);
            }
            else
            {
                thisCamera.nearClipPlane = 0.3f;
                transform.position = Vector3.Lerp(transform.position, new Vector3(position.x, position.y - rvY, position.z + rvZ), Time.deltaTime * orbitDamping);
            }
            transform.rotation = Quaternion.Euler(195, 0, 180);
        } else
        {
            orbitDamping = 10;
            if (player.inRArch)
            {
                thisCamera.nearClipPlane = 2;
                transform.position = Vector3.Lerp(transform.position, new Vector3(8.33f, position.y, position.z), Time.deltaTime * orbitDamping);
            }
            else if (player.inLArch)
            {
                thisCamera.nearClipPlane = 2;
                transform.position = Vector3.Lerp(transform.position, new Vector3(-8.33f, position.y, position.z), Time.deltaTime * orbitDamping);
            }
            else
            {
                thisCamera.nearClipPlane = 0.3f;
                transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * orbitDamping);
            }
            transform.rotation = rotation; //Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * orbitDamping);
        }
    }
}
