using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("Camera Target and Speed")]
    [Tooltip("Target that needs to be followed, speed of the automatic camera")]
    public Transform target;
    public float speed = 0.1f;

    [Header("Velocity of the Charakter")]
    [Tooltip("Gets rigidbody")]
    public Rigidbody2D rb;

    [Header("Procedural Tileset")]
    [Tooltip("Speed at that character moves")]
    public Transform tile1;
    public Transform tile2;
    private float size;


    public void Start()
    {
        size = tile1.GetComponent<BoxCollider2D>().size.y;
    }
    
    //Update the Position of the Camera
    public void Update()
    {
 
        //Make Camera Slower/Faster in context to the Playerspeed
        if(rb.velocity.y <= -2)
        {
            //Camera goes slower (when free falling)
            speed += 0.00005f;
            Vector3 targetPos = new Vector3(0, target.position.y + speed, transform.position.z);
            transform.position = targetPos;
        }
        else
        {
            //Camera goes faster (when hitting an object)
            speed -= 0.0025f;
            Vector3 targetPos = new Vector3(0, target.position.y + speed, transform.position.z);
            transform.position = targetPos;
        }



        //Generate new Tile
        if(transform.position.y < tile1.position.y)
        {
            tile2.position = new Vector3(tile2.position.x, tile1.position.y - size, tile2.position.z);
            SwitchTile();
        }
           
    }

    //Switch the Tiles
    private void SwitchTile()
    {
        Transform temp = tile1;
        tile1 = tile2;
        tile2 = temp;
    }

}
