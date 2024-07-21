using FishNet.Example.ColliderRollbacks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using static UnityEngine.GraphicsBuffer;

public class CameraController : NetworkBehaviour
{
    public float sensitivity = 40.0f;
    public float distance = 12.0f;
    
    private PlayerController player;
    //private float orbitDamping = 10f;
    private float x = 0.0f;
    private float lookRange = 60f;
    private float rvZ = 19.3f;
    private float rvY = 5.2f;
    private Camera thisCamera;

    // Start is called before the first frame update
    void Awake()
    {
        thisCamera = this.gameObject.GetComponent<Camera>();
        player = thisCamera.transform.parent.GetComponent<PlayerController>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            thisCamera = Camera.main;
            thisCamera.transform.SetParent(player.transform);
        }
        else
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject.GetComponent<AudioListener>());
        }
    }

    void LateUpdate()
    {
        if (player != null) 
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
            //orbitDamping = 100;
            if (player.inRArch)
            {
                thisCamera.nearClipPlane = 2;
                transform.position = new Vector3(8.33f, position.y - rvY, position.z + rvZ);
            }
            else if (player.inLArch)
            {
                thisCamera.nearClipPlane = 2;
                transform.position = new Vector3(-8.33f, position.y - rvY, position.z + rvZ);
            }
            else
            {
                thisCamera.nearClipPlane = 0.3f;
                transform.position = new Vector3(position.x, position.y - rvY, position.z + rvZ);
            }
            transform.rotation = Quaternion.Euler(195, 0, 180);
        } else
        {
            //orbitDamping = 10;
            if (player.inRArch)
            {
                thisCamera.nearClipPlane = 2;
                transform.position = new Vector3(8.33f, position.y, position.z);
            }
            else if (player.inLArch)
            {
                thisCamera.nearClipPlane = 2;
                transform.position = new Vector3(-8.33f, position.y, position.z);
            }
            else
            {
                thisCamera.nearClipPlane = 0.3f;
                transform.position = position; // Vector3.Lerp(transform.position, position, Time.deltaTime * orbitDamping);
            }
            transform.rotation = rotation; //Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * orbitDamping);
        }
    }
}
