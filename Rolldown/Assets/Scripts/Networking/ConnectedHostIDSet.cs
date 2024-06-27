using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnectedHostIDSet : MonoBehaviour
{
    private void Start()
    {
        if (TryGetComponent(out TextMeshProUGUI tmp))
            tmp.text = ConnectionManager.GetHostHex();
    }
}
