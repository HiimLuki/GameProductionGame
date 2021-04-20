using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharakterPickable : MonoBehaviour
{
    //Growing Factor
    private float growingFactor = 0.1f;

    /// <summary>
    /// The player movement component on the player
    /// </summary>
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = this.GetComponent<PlayerMovement>();    
    }

    //On Trigger Enter Destroy Pickable and make Charakter bigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Pickable"))
        {
            Destroy(other.gameObject);
            playerMovement.playerWeight += 1f;
            playerMovement.currentTerminalGravity += 0.1f;
            transform.localScale = new Vector3 (transform.localScale.x + growingFactor, transform.localScale.y + growingFactor, transform.localScale.z + growingFactor);
        }
    }
}
