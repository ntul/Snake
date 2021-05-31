using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{

    Vector3 direction;
    float angle = 45;
    public Data data;
    public Camera mainCamera;
    
    private Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (data.IsFever)
        {
            if(transform.position.x>0.1) transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(-angle, Vector3.up), Time.deltaTime * data.turnSpeed);
            else if(transform.position.x<-0.1) transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.up), Time.deltaTime * data.turnSpeed);
            else transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(0, Vector3.up), Time.deltaTime * data.turnSpeed);
            return;
        }

        #if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftArrow))
        {

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(-angle, Vector3.up), Time.deltaTime * data.turnSpeed);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(angle, Vector3.up), Time.deltaTime * data.turnSpeed);

        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(0, Vector3.up), Time.deltaTime * data.turnSpeed);
        }

        #else
        if (Input.touchCount > 0)
        {
            // create ray from the camera and passing through the touch position:
            Ray ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
            // create a logical plane at this object's position
            // and perpendicular to world Y:
            Plane plane = new Plane(Vector3.up, Vector3.up*0.3f);
            float distance = 0; // this will return the distance from the camera
            if (plane.Raycast(ray, out distance))
            { // if plane hit...
                Vector3 pos = ray.GetPoint(distance); // get the point
                if (pos.z > transform.position.z)
                {
                    Vector3 direction = (pos - transform.position).normalized;
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(Vector3.forward, direction), data.turnSpeed * Time.deltaTime);
                    //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(Vector3.Angle(transform.forward, direction), Vector3.up), Time.deltaTime * data.turnSpeed);
                }
                else
                {
                    transform.rotation = pos.x>0?Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(Vector3.forward, Vector3.right), data.turnSpeed * Time.deltaTime)
                        : Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(Vector3.forward, Vector3.left), data.turnSpeed * Time.deltaTime);
                }
                
            }
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(0, Vector3.up), Time.deltaTime * data.turnSpeed);
        }
#endif

    }
    private void FixedUpdate()
    {
        direction = transform.forward  * data.moveSpeed;
        rigidBody.velocity = direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        data.IsGameOver = true;      
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            data.GatesCrossed++;
        }
        else if (other.CompareTag("Dude"))
        {
            MaterialPropertyBlock materialBlock = new MaterialPropertyBlock();
            other.gameObject.GetComponent<Renderer>().GetPropertyBlock(materialBlock);
            if (materialBlock.GetColor("_Color") != data.snakeMat.color)
            {
                if (!data.IsFever) data.IsGameOver = true;
            }

        }
        else if (other.CompareTag("Loader"))
        {
            data.LoaderTrigger();
        }
        else if (!other.CompareTag("Diamond"))
        {
            if (!data.IsFever) data.IsGameOver = true;
        }
        
    }

  
}
