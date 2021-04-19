using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharakterPickable : MonoBehaviour
{
    private float growingFactor = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Pickable"))
        {
            Destroy(other.gameObject);
            
            transform.localScale = new Vector3 (transform.localScale.x + growingFactor, transform.localScale.y + growingFactor, transform.localScale.z + growingFactor);
        }
    }
}
