using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    
    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(0, target.position.y, transform.position.z);

        transform.position = targetPos;
    }
}
