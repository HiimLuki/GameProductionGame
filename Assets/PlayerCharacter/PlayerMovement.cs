using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("General movement related stuff")]
    [Tooltip("Speed at that character moves")]
    public float movementSpeed;
    [Tooltip("The maximal value the y-speed can have through gravity")]
    public float currentTerminalGravity;
    [Tooltip("Friction that should be applied to the horizontal speed of the player")]
    public float friction;
    [Tooltip("Time where no input is possible when using dash")]
    public float dashDuration;
    [Tooltip("Force of dash")]
    public float dashForce = 5f;

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

    public enum PlayerStates
    {
        FALLING,
        MOVING,
        DASHING
    }
    [Header("Player States")]
    [Tooltip("The current movement state of the player")]
    public PlayerStates currentPlayerState;

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
    /// <summary>
    /// Value to set max drag (-> max gravity) for player
    /// Is related to terminalGravity and should be equal terminal
    /// gravity except in specific situations
    /// </summary>
    private float terminalVelocityY;
    /// <summary>
    /// This vector uses mousePosition to create a vector in which direction player can dash to
    /// </summary>
    private Vector3 dashVector;
    /// <summary>
    /// For counting dashDuration
    /// </summary>
    private float dashCooldown;

    // Called before Start
    private void Awake()
    {
        for(int i = 0; i < this.transform.childCount; i++)
        {
            CharakterPickable charakterPickable = this.transform.GetChild(0).gameObject.AddComponent<CharakterPickable>();
            charakterPickable.playerMovement = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.targetVelocity = this.rb.velocity;
        this.mousePosition = this.transform.position;
        this.horizontalSpeed = 0f;
        this.dashCooldown = 0f;
        terminalVelocityY = this.currentTerminalGravity;
    }

    // Update is called once per frame
    void Update()
    {
        // If player is in dashing state wait until duration of
        // dash is over before proceeding with normal movement caluclation
        if (this.currentPlayerState == PlayerStates.DASHING)
        {
            if (dashDuration > dashCooldown)
            {
                dashCooldown += 1f * Time.deltaTime;
            } else
            {
                dashCooldown = 0f;
                this.terminalVelocityY = currentTerminalGravity;
                this.currentPlayerState = PlayerStates.FALLING;
            }
        }

        // Character can only move when not in DASHING state
        if(this.currentPlayerState != PlayerStates.DASHING)
        {
            // After dash cooldown calculation init frame like normal
            InitFrame();

            // Pressing F slows character and we can dash in a direction
            if (Input.GetKey(KeyCode.F))
            {
                this.terminalVelocityY = 0.1f;
                dashVector = this.mousePosition;
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                // Character is in Dash mode
                this.currentPlayerState = PlayerStates.DASHING;
                rb.AddForce(dashVector * dashForce, ForceMode2D.Impulse);
                return;
            }

            // Player can move character with mouse
            if (Input.GetMouseButton(0))
            {
                // TODO: Should player only be in movement mode when 
                // player interaction is present or when horizontal movementSpeed != 0?
                this.currentPlayerState = PlayerStates.MOVING;
                if (this.transform.position.x < this.mousePosition.x)
                {
                    this.horizontalSpeed = 1 * movementSpeed;
                }
                if (this.transform.position.x > this.mousePosition.x)
                {
                    this.horizontalSpeed = -1 * movementSpeed;
                }
            }
            // Player can also move character with A & D
            if (Input.GetKey(KeyCode.A))
            {
                this.currentPlayerState = PlayerStates.MOVING;
                this.horizontalSpeed = -1 * movementSpeed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                this.currentPlayerState = PlayerStates.MOVING;
                this.horizontalSpeed = 1 * movementSpeed;
            }

            // Shrink player on button press
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // If player is still above allowed minimum weight, it can shrink further
                if (this.playerWeight > minimumPlayerWeight)
                {
                    ShrinkPlayer();
                }
            }

            // Apply constant falling velocity
            // Calculating terminal velocity by adjusting drag of rigidbody
            this.rb.drag = GetDragFromAcceleration(Physics.gravity.magnitude, terminalVelocityY);

            // Apply friction
            if (this.horizontalSpeed > 0)
            {
                this.horizontalSpeed -= this.friction * Time.deltaTime;
            }
            else if (this.horizontalSpeed < 0)
            {
                this.horizontalSpeed += this.friction * Time.deltaTime;
            }

            // Apply horizontal movement speed if applicable
            this.targetVelocity = new Vector2(this.horizontalSpeed, this.rb.velocity.y);

            // Apply velocity to rigidbody
            this.rb.velocity = this.targetVelocity;
        }
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
    /// All values that need to be set at the beginning of every frame 
    /// should be set in here
    /// </summary>
    private void InitFrame()
    {
        // By default x velocity of player should be taken from its rigidbody
        this.horizontalSpeed = rb.velocity.x;
        // By default terminal velocity is equal current terminal gravity
        this.terminalVelocityY = currentTerminalGravity;
        // Mouse position is needed every frame
        this.mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // By default character is in falling state every frame
        this.currentPlayerState = PlayerStates.FALLING;
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
