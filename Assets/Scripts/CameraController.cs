using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    //Look At Variable
    public Transform target;
    
    //Update the Position of the Camera
    public void Update()
    {
        Vector3 targetPos = new Vector3(0, target.position.y, transform.position.z);

        transform.position = targetPos;
    }
}
