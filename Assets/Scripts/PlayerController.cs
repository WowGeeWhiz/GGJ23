using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //player variables
    public float speed; //player movement speed
    public Collider2D leftAttack, rightAttack, upAttack, downAttack; //hitboxes for directional attacks
    public Collider2D leftUpAttack, leftDownAttack, rightUpAttack, rightDownAttack; //hitboxes for diagonal attacks
    public bool canAttack = true, lockMovement = false; //bool on if the player can currently attack (to be set false when in build menu)
    public float damage;

    public float score;
    public TextMeshProUGUI scoreText;

    bool attacking = false; //private bool for currently attacking
    bool movedUp, movedDown, movedLeft, movedRight, movedLeftUp, movedLeftDown, movedRightUp, movedRightDown; //directional bools
    Vector2 movement; //variable for not moving
    Rigidbody2D rb; //player rigidbody
    Collider2D col; //player hitbox
    BuildingSystem buildSys; //building controls

    // Start is called before the first frame update
    void Start()
    {
        movedLeft = true;
        DisableAttacks(); //disable the attack hitboxes
        
        //get the components
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        buildSys = GetComponent<BuildingSystem>();
        Debug.Log("Startup Complete");

        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (lockMovement)
        {
            movement = Vector2.zero;
            return;
        }

        canAttack = !buildSys.buildModeActive;

        //check for current input
        Move();

        //check for attacking
        if (canAttack && Input.GetKey(KeyCode.Mouse0)) Attack();
        //if not attacking, disable the attack hitboxes
        else if (attacking) DisableAttacks();


        scoreText.text = "Score: " + score.ToString();
    }

    //set the hitboxes invisible
    private void DisableAttacks(string ignore = "")
    {
        //Debug.Log("Disabled attack hitboxes");
        if (!ignore.Equals("left")) leftAttack.gameObject.SetActive(false);
        if (!ignore.Equals("right")) rightAttack.gameObject.SetActive(false);
        if (!ignore.Equals("up")) upAttack.gameObject.SetActive(false);
        if (!ignore.Equals("down")) downAttack.gameObject.SetActive(false);
        if (!ignore.Equals("leftUp")) leftUpAttack.gameObject.SetActive(false);
        if (!ignore.Equals("leftDown")) leftDownAttack.gameObject.SetActive(false);
        if (!ignore.Equals("rightUp")) rightUpAttack.gameObject.SetActive(false);
        if (!ignore.Equals("rightDown")) rightDownAttack.gameObject.SetActive(false);
        attacking = false;
    }

    //set the hitbox visible
    private void Attack()
    {
        if (attacking) return;
        //Debug.Log("Attacked");

        if (movedLeft) leftAttack.gameObject.SetActive(true);
        if (movedLeftUp) leftUpAttack.gameObject.SetActive(true);
        if (movedLeftDown) leftDownAttack.gameObject.SetActive(true);
        if (movedRight) rightAttack.gameObject.SetActive(true);
        if (movedRightUp) rightUpAttack.gameObject.SetActive(true);
        if (movedRightDown) rightDownAttack.gameObject.SetActive(true);
        if (movedUp) upAttack.gameObject.SetActive(true);
        if (movedDown) downAttack.gameObject.SetActive(true);
        //Debug.Log("Attacked but no moved is true");
        attacking = true;
    }

    private void FixedUpdate()
    {
        //apply movement Vector2 to transform at fixed rate
        //this is for turning without moving
        if (Input.GetKey(KeyCode.LeftShift)) rb.MovePosition(rb.position + movement * speed * 0 * Time.fixedDeltaTime);
        //this is normal movement speed
        else rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    //set the movement Vector2 and the directional bools
    void Move()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        float y = movement.y;
        float x = movement.x;

        if (y > 0)
        {
            if (x == 0)
            {
                movedUp = true;
                movedLeftUp = movedRightUp = movedDown = movedRightDown = movedLeftDown = movedRight = movedLeft = false;
            }
            else if (x < 0)
            {
                movedLeftUp = true;
                movedUp = movedRightUp = movedDown = movedRightDown = movedLeftDown = movedRight = movedLeft = false;
            }
            else
            {
                movedRightUp = true;
                movedUp = movedLeftUp = movedDown = movedRightDown = movedLeftDown = movedRight = movedLeft = false;
            }
        }
        else if (y < 0)
        {
            if (x == 0)
            {
                movedDown = true;
                movedUp = movedLeftUp = movedRightUp = movedRightDown = movedLeftDown = movedRight = movedLeft = false;
            }
            else if (x < 0)
            {
                movedLeftDown = true;
                movedUp = movedLeftUp = movedRightUp = movedDown = movedRightDown = movedRight = movedLeft = false;
            }
            else
            {
                movedRightDown = true;
                movedUp = movedLeftUp = movedRightUp = movedDown = movedLeftDown = movedRight = movedLeft = false;
            }
        }
        else
        {
            if (x > 0)
            {
                movedRight = true;
                movedUp = movedLeftUp = movedRightUp = movedDown = movedRightDown = movedLeftDown = movedLeft = false;

            }
            else if (x < 0)
            {
                movedLeft = true;
                movedUp = movedLeftUp = movedRightUp = movedDown = movedRightDown = movedLeftDown = movedRight = false;

            }

        }
    }
}
