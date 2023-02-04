using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //player variables
    public float speed, sprintModifier; //player movement speed
    public Collider2D leftAttack, rightAttack, upAttack, downAttack; //hitboxes for directional attacks
    public bool canAttack = true; //bool on if the player can currently attack (to be set false when in build menu)
    
    bool attacking = false; //private bool for currently attacking
    bool movedUp, movedDown, movedLeft, movedRight; //directional bools
    Vector2 movement; //variable for not moving
    Rigidbody2D rb; //player rigidbody
    Collider2D col; //player hitbox

    // Start is called before the first frame update
    void Start()
    {
        movedLeft = true;
        DisableAttacks(); //disable the attack hitboxes
        
        //get the components
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        Debug.Log("Startup Complete");
    }

    // Update is called once per frame
    void Update()
    {
        //check for current input
        Move();

        //check for attacking
        if (canAttack && Input.GetKey(KeyCode.Mouse0)) Attack();
        //if not attacking, disable the attack hitboxes
        else if (attacking) DisableAttacks();
    }

    //set the hitboxes invisible
    private void DisableAttacks(string ignore = "")
    {
        Debug.Log("Disabled attack hitboxes");
        if (!ignore.Equals("left")) leftAttack.gameObject.SetActive(false);
        if (!ignore.Equals("right")) rightAttack.gameObject.SetActive(false);
        if (!ignore.Equals("up")) upAttack.gameObject.SetActive(false);
        if (!ignore.Equals("down")) downAttack.gameObject.SetActive(false);
        attacking = false;
    }

    //set the hitbox visible
    private void Attack()
    {
        if (attacking) return;
        Debug.Log("Attacked");
        if (movedLeft) leftAttack.gameObject.SetActive(true);
        if (movedUp) upAttack.gameObject.SetActive(true);
        if (movedDown) downAttack.gameObject.SetActive(true);
        if (movedRight) rightAttack.gameObject.SetActive(true);
        attacking = true;
    }

    private void FixedUpdate()
    {
        //apply movement Vector2 to transform at fixed rate
        //this is for sprinting
        if (Input.GetKey(KeyCode.LeftShift)) rb.MovePosition(rb.position + movement * speed * sprintModifier * Time.fixedDeltaTime);
        //this is normal movement speed
        else rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    //set the movement Vector2 and the directional bools
    void Move()
    {
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.y > 0)
        {
            movedUp = true;
            movedDown = movedLeft = movedRight = false;
        }
        else if (movement.y < 0)
        {
            movedDown = true;
            movedUp = movedLeft = movedRight = false;
        }
        movement.x = Input.GetAxisRaw("Horizontal");
        if (movement.x > 0)
        {
            movedRight = true;
            movedUp = movedLeft = movedDown = false;
        }
        else if (movement.x < 0)
        {
            movedLeft = true;
            movedRight = movedDown = movedUp = false;
        }
    }
}
