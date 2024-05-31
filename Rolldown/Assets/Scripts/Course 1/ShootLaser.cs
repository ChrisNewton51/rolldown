using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System;
using UnityEngine;

public class ShootLaser : MonoBehaviour
{
    public int damageOverTime = 30;

    public GameObject HitEffect;
    public float HitOffset = 0;
    public bool useLaserRotation = false;

    public float MaxLength;
    private LineRenderer Laser;

    public float MainTextureLength = 1f;
    public float NoiseTextureLength = 1f;
    private Vector4 Length = new Vector4(1,1,1,1);
    //private Vector4 LaserSpeed = new Vector4(0, 0, 0, 0); {DISABLED AFTER UPDATE}
    //private Vector4 LaserStartSpeed; {DISABLED AFTER UPDATE}
    //One activation per shoot
    private bool LaserSaver = false;
    private bool UpdateSaver = false;

    private ParticleSystem[] Effects;
    private ParticleSystem[] Hit;
    private PlayerController player;
    private BoxCollider laserCollider;
    private int playerLayer;
    private int layerMask;

    void Start ()
    {
        //Get LineRender and ParticleSystem components from current prefab;  
        Laser = GetComponent<LineRenderer>();
        Effects = GetComponentsInChildren<ParticleSystem>();
        Hit = HitEffect.GetComponentsInChildren<ParticleSystem>();

        laserCollider = gameObject.AddComponent<BoxCollider>();
        laserCollider.isTrigger = true;

        player = GetComponentInParent<PlayerController>();

        playerLayer = LayerMask.NameToLayer("Player");
        layerMask = ~(1 << playerLayer);

    }

    void Update()
    {
        Vector3 directionToTarget = (player.target.transform.position - transform.position).normalized;

        Laser.material.SetTextureScale("_MainTex", new Vector2(Length[0], Length[1]));                    
        Laser.material.SetTextureScale("_Noise", new Vector2(Length[2], Length[3]));
        
        //To set LineRender position
        if (Laser != null && UpdateSaver == false)
        {
            Laser.SetPosition(0, transform.position);
            RaycastHit hit;     
            if (Physics.Raycast(transform.position, directionToTarget, out hit, MaxLength, layerMask))
            {
                //End laser position if collides with object
                Laser.SetPosition(1, hit.point);

                HitEffect.transform.position = hit.point + hit.normal * HitOffset;
                if (useLaserRotation)
                    HitEffect.transform.rotation = transform.rotation;
                else
                    HitEffect.transform.LookAt(hit.point + hit.normal);

                foreach (var AllPs in Effects)
                {
                    if (!AllPs.isPlaying) AllPs.Play();
                }
                //Texture tiling
                Length[0] = MainTextureLength * (Vector3.Distance(transform.position, hit.point));
                Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, hit.point));

                UpdateLaserCollider(hit.point);
            }
            else
            {
                //End laser position if doesn't collide with object
                var EndPos = transform.position + directionToTarget * MaxLength;
                Laser.SetPosition(1, EndPos);
                HitEffect.transform.position = EndPos;
                foreach (var AllPs in Hit)
                {
                    if (AllPs.isPlaying) AllPs.Stop();
                }
                //Texture tiling
                Length[0] = MainTextureLength * (Vector3.Distance(transform.position, EndPos));
                Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, EndPos));

                UpdateLaserCollider(EndPos);
            }
            //Insurance against the appearance of a laser in the center of coordinates!
            if (Laser.enabled == false && LaserSaver == false)
            {
                LaserSaver = true;
                Laser.enabled = true;
            }
        }  
    }

    void UpdateLaserCollider(Vector3 endPoint)
    {
        // Calculate the center position of the BoxCollider
        Vector3 centerPosition = (transform.position + endPoint) / 2;

        // Calculate the size of the BoxCollider
        float laserLength = Vector3.Distance(transform.position, endPoint);
        Vector3 colliderSize = new Vector3(Laser.startWidth, Laser.startWidth, laserLength + 1);

        // Update the BoxCollider properties
        laserCollider.size = colliderSize;
        laserCollider.center = transform.InverseTransformPoint(centerPosition);

        // Align the BoxCollider with the laser direction
        laserCollider.transform.LookAt(endPoint);
    }

    public void DisablePrepare()
    {
        if (Laser != null)
        {
            Laser.enabled = false;
        }
        UpdateSaver = true;
        //Effects can = null in multiply shooting
        if (Effects != null)
        {
            foreach (var AllPs in Effects)
            {
                if (AllPs.isPlaying) AllPs.Stop();
            }
        }
    }
}
