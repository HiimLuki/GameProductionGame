using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableEntity : MonoBehaviour
{
    [Header("Breakable Entity Base Stats")]
    [Tooltip("The weight the player has to have to break this entity")]
    public float breakableWeight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Get PlayerMovement script to get current weight of player
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovement == null) return;
            if (playerMovement.playerWeight > this.breakableWeight)
            {
                Destroy(this.gameObject);
                // TODO: This should of course properly break, animated and stuff :D 
            }
        }
    }
}
