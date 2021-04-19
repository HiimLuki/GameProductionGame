using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Speed at that character moves")]
    public float movementSpeed;

    [Tooltip("Gravity that pulls the character down")]
    public float gravity;

    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = transform.position.z;
        }

        targetPosition.y -= gravity;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
    }
}
