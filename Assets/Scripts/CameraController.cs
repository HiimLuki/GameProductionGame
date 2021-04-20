using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    //Look At Variable
    public Transform target;
    private float speed = 0.1f;

    public Rigidbody2D rb;
    
    //Update the Position of the Camera
    public void Update()
    {
 
        if(rb.velocity.y <= -2)
        {
            speed += 0.001f;
            Vector3 targetPos = new Vector3(0, target.position.y + speed, transform.position.z);
            transform.position = targetPos;
        }
        else
        {
            speed -= 0.001f;
            Vector3 targetPos = new Vector3(0, target.position.y + speed, transform.position.z);
            transform.position = targetPos;
        }
           
    }

}
