using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    //Look At Variable
    public Transform target;
    private float speed = 0.1f;

    //Variable to get Velocity
    public Rigidbody2D rb;

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
 
        //Make Camera Slower Faster in context to the Playerspeed
        if(rb.velocity.y <= -2)
        {
            //Camera goes slower
            speed += 0.001f;
            Vector3 targetPos = new Vector3(0, target.position.y + speed, transform.position.z);
            transform.position = targetPos;
        }
        else
        {
            //Camera goes faster
            speed -= 0.001f;
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
