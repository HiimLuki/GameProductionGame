using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharakterPickable : MonoBehaviour
{
    //Growing Factor
    private float growingFactor = 0.1f;

    //On Trigger Enter Destroy Pickable and make Charakter bigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Pickable"))
        {
            Destroy(other.gameObject);
            
            transform.localScale = new Vector3 (transform.localScale.x + growingFactor, transform.localScale.y + growingFactor, transform.localScale.z + growingFactor);
        }
    }
}
