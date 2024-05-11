using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePanel : MonoBehaviour
{
    private float sideBound = 45;
    private float topBound = -10;
    private float moveSpeed = 8;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveSidePanels();
    }

    void MoveSidePanels()
    {
        if (gameObject.name == "Side Panel")
        {
            if (transform.position.x < sideBound)
            {
                transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            }
            else
            {
                transform.position = new Vector3(-sideBound, transform.position.y, transform.position.z);
            }
        }
        if (gameObject.name == "SideR Panel")
        {
            if (transform.position.x >= -sideBound)
            {
                transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
            }
            else
            {
                transform.position = new Vector3(sideBound, transform.position.y, transform.position.z);
            }
        }

    }

    void MoveVertPanels()
    {
        if (gameObject.name == "Vert Panel")
        {
            if (transform.position.x <= topBound)
            {
                transform.Translate(Vector3.down * Time.deltaTime * moveSpeed);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, -topBound, transform.position.z);
            }
        }
    }
}
