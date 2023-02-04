using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //player variables
    public int speed; //player movement speed
    public Rigidbody2D rb; //player rigidbody
    public Collider2D col; //player hitbox
    private bool movedUp, movedDown, movedLeft, movedRight; //directional bools
    public Collider2D leftAttack, rightAttack, upAttack, downAttack;
    internal bool attacking = false;
    public float attackHideDelay;
    internal float lastAttackTime;

    // Start is called before the first frame update
    void Start()
    {
        DisableAttacks();
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
                movedUp = true;
                movedLeft = movedRight = movedDown = false;
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
            movedDown = true;
            movedLeft = movedRight = movedUp = false;
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
                movedLeft = true;
                movedUp = movedDown = movedRight = false;
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
            movedRight = true;
            movedUp = movedDown = movedLeft = false;
        }


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
}
