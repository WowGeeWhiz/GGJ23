using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        //move up if pressing up and not pressing down
        if (Input.GetKey(KeyCode.W))
        {
            if (!Input.GetKey(KeyCode.S))
            {
                rb.transform.position =
                    new Vector3
                    (rb.transform.position.x,
                    rb.transform.position.y + Time.deltaTime * speed,
                    rb.transform.position.z);
            }
        }
        //move down if pressing down and not pressing up
        else if (Input.GetKey(KeyCode.S))
        {
            rb.transform.position =
                new Vector3
                (rb.transform.position.x,
                rb.transform.position.y - Time.deltaTime * speed,
                rb.transform.position.z);
        }

        //move left if pressing left and not right
        if (Input.GetKey(KeyCode.A))
        {
            if (!Input.GetKey(KeyCode.D))
            {
                rb.transform.position =
                    new Vector3
                    (rb.transform.position.x - Time.deltaTime * speed,
                    rb.transform.position.y,
                    rb.transform.position.z);
            }
        }
        //move right if pressing right and not left
        else if (Input.GetKey(KeyCode.D))
        {
            rb.transform.position =
                new Vector3
                (rb.transform.position.x + Time.deltaTime * speed,
                rb.transform.position.y,
                rb.transform.position.z);
        }

    }
}
