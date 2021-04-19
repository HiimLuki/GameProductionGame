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

    [Header("Character Shrinking")]
    [Tooltip("How much the character should shrink each time ability is used")]
    public float shrinkFactor;

    private Vector3 targetPosition;

    private Rigidbody2D rb;

    private Vector3 size;

    // Start is called before the first frame update
    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.size = this.transform.localScale;
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
            rb.AddForce(new Vector2(targetPosition.x, targetPosition.y));
        }

        // Shrink player on button press
        if(Input.GetKeyDown(KeyCode.Space))
        {
            this.transform.localScale = new Vector3(
                this.transform.localScale.x - shrinkFactor, 
                this.transform.localScale.y - shrinkFactor, 
                this.transform.localScale.z - shrinkFactor);
        }
    }
}
