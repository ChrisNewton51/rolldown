using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float sensitivity = 3.0f;
    private Vector3 offset = new Vector3(0,6,-6);
    private Vector3 localRot;
    private float orbitDamping = 5f;
    private float distanceToPlayer;


    // Start is called before the first frame update
    void Start()
    {

    }

    void LateUpdate()
    {
        // Follow player
        transform.position = player.transform.position + offset;

        // Rotate camera around player
        if (Input.GetKey(KeyCode.Mouse1))
        {
            CamOrbit();
        }
    }

    private void CamOrbit() 
    {
        localRot.x += Input.GetAxis("Mouse X") * sensitivity;
        localRot.y -= Input.GetAxis("Mouse Y") * sensitivity;

        localRot.y = Mathf.Clamp(localRot.y, 15f, 60f);
        localRot.x = Mathf.Clamp(localRot.x, -45f, 45f);

        Quaternion QT = Quaternion.Euler(localRot.y, localRot.x, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, QT, Time.deltaTime * orbitDamping);
    }
}
