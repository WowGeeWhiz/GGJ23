using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //player variables
    public int speed; //player movement speed
    public Collider2D leftAttack, rightAttack, upAttack, downAttack; //hitboxes for directional attacks

    bool attacking = false; //private bool for currently attacking
    bool movedUp, movedDown, movedLeft, movedRight; //directional bools
    Vector2 movement; //variable for not moving
    Rigidbody2D rb; //player rigidbody
    Collider2D col; //player hitbox

    // Start is called before the first frame update
    void Start()
    {
        DisableAttacks(); //disable the attack hitboxes
        //get the component
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKey(KeyCode.Mouse0)) Attack();
        else if (attacking) DisableAttacks();
    }

    //set the hitboxes invisible
    private void DisableAttacks(string ignore = "")
    {
        Debug.Log("Disabled objects");
        if (!ignore.Equals("left")) leftAttack.gameObject.SetActive(false);
        if (!ignore.Equals("right")) rightAttack.gameObject.SetActive(false);
        if (!ignore.Equals("up")) upAttack.gameObject.SetActive(false);
        if (!ignore.Equals("down")) downAttack.gameObject.SetActive(false);
        attacking = false;
    }

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
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    void Move()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }
}
