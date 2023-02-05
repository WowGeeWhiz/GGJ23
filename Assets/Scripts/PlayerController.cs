using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Timers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class PlayerController : MonoBehaviour
{
    public bool resetHouse = false;
    private SpriteRenderer playerSprite;
    public VideoClip spawn, death, respawn;
    public VideoPlayer vp;
    //player variables
    Animator animator;
    public float speed; //player movement speed
    public Collider2D leftAttack, rightAttack, upAttack, downAttack; //hitboxes for directional attacks
    public Collider2D leftUpAttack, leftDownAttack, rightUpAttack, rightDownAttack; //hitboxes for diagonal attacks
    public bool canAttack = true, lockMovement = false; //bool on if the player can currently attack (to be set false when in build menu)
    public float damage;
    public bool GodMode;
    private bool hasRestored;
    private float restoreAt;
    public GameObject[] cinemEnables;

    public bool autoKillEnemies = false;

    public float score;
    public TextMeshProUGUI scoreText;

    public int wood;
    public TextMeshProUGUI woodText;

    bool attacking = false; //private bool for currently attacking
    bool movedUp, movedDown, movedLeft, movedRight, movedLeftUp, movedLeftDown, movedRightUp, movedRightDown; //directional bools
    Vector2 movement; //variable for not moving
    Rigidbody2D rb; //player rigidbody
    Collider2D col; //player hitbox
    BuildingSystem buildSys; //building controls

    float stepTimer, stepDelay, stepRate;
    private AudioSource audioSource;
    public AudioClip[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();
        movedLeft = true;
        DisableAttacks(); //disable the attack hitboxes
        
        //get the components
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        buildSys = GetComponent<BuildingSystem>();
        Debug.Log("Startup Complete");

        score = 0;
        wood = 0;

        audioSource = GetComponent<AudioSource>();

        stepTimer = 0;
        stepRate = 2f;
        stepDelay = 1 / stepRate;




        PlaySpawn();
    }

    // Update is called once per frame
    void Update()
    {

        if (!hasRestored && Time.fixedTime >= restoreAt)
        {
            lockMovement = false;
            playerSprite.enabled = true;
            foreach (GameObject obj in cinemEnables) obj.SetActive(true);
            vp.clip = null;
            hasRestored = true;
        }


        if (lockMovement && !GodMode)
        {
            movement = Vector2.zero;
            return;
        }

        canAttack = !buildSys.buildModeActive && !buildSys.removeModeActive;

        //check for current input
        Move();

        //check for attacking
        if (canAttack && Input.GetKey(KeyCode.Mouse0)) Attack();
        //if not attacking, disable the attack hitboxes
        else if (attacking) DisableAttacks();


        scoreText.text = "Score: " + score.ToString();
        woodText.text  = wood.ToString();
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
        animator.SetBool("isAttacking", false);
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
        animator.SetBool("isAttacking", true);

        PlayAudio(0, 7);
    }

    private void FixedUpdate()
    {
        //apply movement Vector2 to transform at fixed rate
        //this is for turning without moving
        if (Input.GetKey(KeyCode.LeftShift)) rb.MovePosition(rb.position + movement * speed * 0 * Time.fixedDeltaTime);
        //this is normal movement speed
        else rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        if(movement.x != 0 || movement.y != 0)
        {
            animator.SetBool("isWalking", true);
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0)
            {
                stepTimer = stepDelay;
                PlayAudio(8, 21);
            }

        }
        else
        {
            animator.SetBool("isWalking", false);
        }

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

    public void PlayAudio(int startIndex, int endIndex)
    {
        int index = Random.Range(startIndex, endIndex);
        audioSource.clip = sounds[index];
        audioSource.PlayOneShot(audioSource.clip, 0.7f);
    }

    public void PlaySpawn()
    {
        autoKillEnemies = false;
        lockMovement = true;
        hasRestored = false;
        playerSprite.enabled = false;
        vp.clip = spawn;
        vp.Play();
        foreach (GameObject obj in cinemEnables) obj.SetActive(false);
        restoreAt = Time.fixedTime + (float)vp.clip.length;
    }

    public void PlayDeath()
    {
        autoKillEnemies = true;
        lockMovement = true;
        hasRestored = false;
        playerSprite.enabled = false;
        vp.clip = death;
        vp.Play();
        foreach (GameObject obj in cinemEnables) obj.SetActive(false);
        restoreAt = Time.fixedTime + (float)vp.clip.length;
    }

    public void PlayRespawn()
    {

        lockMovement = true;
        hasRestored = false;
        playerSprite.enabled = false;
        vp.clip = respawn;
        vp.Play();
        foreach (GameObject obj in cinemEnables) obj.SetActive(false);
        restoreAt = Time.fixedTime + (float)vp.clip.length;
    }
}
