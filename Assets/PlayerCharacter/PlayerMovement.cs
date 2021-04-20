using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("General movement related stuff")]
    [Tooltip("Speed at that character moves")]
    public float movementSpeed;
    [Tooltip("The maximal value the y-speed can have through gravity")]
    public float terminalVelocityY;
    [Tooltip("Friction that should be applied to the horizontal speed of the player")]
    public float friction;

    [Header("Character Shrinking")]
    [Tooltip("How much the character should shrink each time ability is used")]
    public float shrinkFactor;
    [Tooltip("The current weight of the player")]
    public float playerWeight = 10f;
    [Tooltip("The minimum weight the player can have. Cannot shrink further at this point")]
    public float minimumPlayerWeight = 5f;

    [Header("Minions")]
    [Tooltip("This should be the minion gameObject prefab")]
    public GameObject minion;
    [Tooltip("How many minions are created on each split attempt")]
    public int minionSplitCounter = 8;

    /// <summary>
    /// Vector that is used to calculate new velocity to apply to rigidbody
    /// </summary>
    private Vector3 targetVelocity;
    /// <summary>
    /// Used to calculate the horizontal speed that should be applied as velocity
    /// to the rigidbody of the player controller
    /// </summary>
    private float horizontalSpeed;
    /// <summary>
    /// Mouse position from main camera to world point
    /// </summary>
    private Vector3 mousePosition;
    /// <summary>
    /// Rigidbody of the player character
    /// </summary>
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.targetVelocity = this.rb.velocity;
        this.mousePosition = this.transform.position;
        this.horizontalSpeed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // By default x velocity of player should be taken from its rigidbody
        this.horizontalSpeed = rb.velocity.x;

        // Player can move character with mouse
        if (Input.GetMouseButton(0))
        {
            this.mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(this.transform.position.x < this.mousePosition.x)
            {
                this.horizontalSpeed = 1 * movementSpeed;
            }
            if(this.transform.position.x > this.mousePosition.x)
            {
                this.horizontalSpeed = -1 * movementSpeed;
            }
        }
        // Player can also move character with A & D
        if(Input.GetKey(KeyCode.A))
        {
            this.horizontalSpeed = -1 * movementSpeed;
        }
        if(Input.GetKey(KeyCode.D))
        {
            this.horizontalSpeed = 1 * movementSpeed;
        }

        // Shrink player on button press
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // If player has not used all shrink attempts shrink player
            if(this.playerWeight > minimumPlayerWeight)
            {
                ShrinkPlayer();
            }
        }

        // Apply constant falling velocity
        // Calculating terminal velocity by adjusting drag of rigidbody
        this.rb.drag = GetDragFromAcceleration(Physics.gravity.magnitude, terminalVelocityY);

        // Apply friction
        if(this.horizontalSpeed > 0)
        {
            this.horizontalSpeed -= this.friction * Time.deltaTime;
        } else if(this.horizontalSpeed < 0)
        {
            this.horizontalSpeed += this.friction * Time.deltaTime;
        }

        // Apply horizontal movement speed if applicable
        this.targetVelocity = new Vector2(this.horizontalSpeed, this.rb.velocity.y);

        // Apply velocity to rigidbody
        this.rb.velocity = this.targetVelocity;
    }


    public void ShrinkPlayer()
    {
        this.transform.localScale = new Vector3(
            this.transform.localScale.x - shrinkFactor,
            this.transform.localScale.y - shrinkFactor,
            this.transform.localScale.z - shrinkFactor);
        // Every time the player shrinks the terminal velocity y should get lower
        // so the player falls slower the smaller it is
        this.terminalVelocityY -= shrinkFactor;
        this.playerWeight -= 1f;
        if(this.minion != null)
        {
            InstantiateMinions();
        }
    }

    public void InstantiateMinions()
    {
        for(int i = 0; i < minionSplitCounter; i++)
        {
            Instantiate(minion, this.transform.position, Quaternion.identity);
        }
    }


    /// <summary>
    /// Helper function to get calculate dragging from two floats
    /// </summary>
    /// <param name="aVelocityChange"></param>
    /// <param name="aFinalVelocity"></param>
    /// <returns></returns>
    private static float GetDrag(float aVelocityChange, float aFinalVelocity)
    {
        return aVelocityChange / ((aFinalVelocity + aVelocityChange) * Time.fixedDeltaTime);
    }
    /// <summary>
    /// Helper function to calculate dragging with acceleration
    /// </summary>
    /// <param name="aAcceleration"></param>
    /// <param name="aFinalVelocity"></param>
    /// <returns></returns>
    private static float GetDragFromAcceleration(float aAcceleration, float aFinalVelocity)
    {
        return GetDrag(aAcceleration * Time.fixedDeltaTime, aFinalVelocity);
    }
}
