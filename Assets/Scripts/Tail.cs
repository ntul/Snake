using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{

    public GameObject leader;
    public Data data;

    private Vector3 direction;
    private Rigidbody rigidBody;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        speed = data.moveSpeed;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, leader.transform.position) < 1.5f*transform.localScale.x)
        {
            speed *= 0.8f;
        }
        else speed = data.moveSpeed;
            
        transform.LookAt(leader.transform);
        direction = transform.forward * speed;
        rigidBody.velocity = direction;
    }

}