using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Material material;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Disable()
    {
        StartCoroutine(Invincible());
    }

    IEnumerator Invincible()
    {
        float time = Time.time;
        Color emColor = material.GetColor("_EmissionColor");
        float waitTime = 0.15f;
        for (int i = 0; i < 6; i++)
        {
            material.SetColor("_EmissionColor", emColor * 0.5f);
            material.SetColor("_BaseColor", new Color(material.color.r - 20, material.color.g - 20, material.color.b - 20, 0.2f));
            yield return new WaitForSeconds(waitTime);
            material.SetColor("_EmissionColor", emColor * 1);
            material.SetColor("_BaseColor", Color.black);
            yield return new WaitForSeconds(waitTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Laser"))
        {
            Disable();
        }
    }
}
