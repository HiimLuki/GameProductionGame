using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("General movement related stuff")]
    [Tooltip("Speed at that character moves")]
    public float movementSpeed;
    [Tooltip("Layer that detects collision for player")]
    public LayerMask collisionMask;
    [Tooltip("Collision detection distance")]
    public float collisionDetectionDistance;

    [Header("Gravity related stuff")]
    [Tooltip("Gravity that pulls the character down")]
    public float gravity;
    [Tooltip("Mass of the player character.")]
    public float mass;

    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        this.targetPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Player can move character with mouse
        if(Input.GetMouseButton(0))
        {
            this.targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.targetPosition.z = this.transform.position.z;
            this.targetPosition.y = this.transform.position.y;
        }

        // Apply gravity to character
        this.targetPosition.y -= gravity;

        // Apply new position to character
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetPosition, this.movementSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, collisionDetectionDistance, collisionMask);

        // If it hits something...
        if (hit.collider != null)
        {
            Debug.Log("Hit wall: " + hit.collider.gameObject.name);
        }
    }
}
