using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject head;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, head.transform.position.z - 4.5f);
    }
}
