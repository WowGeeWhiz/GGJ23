using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //player variables
    public int speed; //player movement speed
    public Rigidbody2D rb; //player rigidbody
    public Collider2D col; //player hitbox

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move up
        if (Input.GetKey(KeyCode.W))
        {
            rb.transform.position =
                new Vector3
                (rb.transform.position.x,
                rb.transform.position.y + Time.deltaTime * speed,
                rb.transform.position.z);
        }
        //move down
        else if (Input.GetKey(KeyCode.S))
        {
            rb.transform.position =
                new Vector3
                (rb.transform.position.x,
                rb.transform.position.y - Time.deltaTime * speed,
                rb.transform.position.z);
        }
    }
}
