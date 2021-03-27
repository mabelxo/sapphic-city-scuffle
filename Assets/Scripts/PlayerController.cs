using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Speed of character movement and height of the jump. Set these values in the inspector.
    public float moveSpeed;
    public float dodgeForce;
    public float startDodgeTime;
    public float maxStamina = 100.0f;
    public Animator playerAnimator;
    public SpriteRenderer playerSprite;
    public AudioSource jumpSource;

    //Public combat variables
    public Transform punchPoint;
    public Transform kickPoint;
    public LayerMask enemyLayers;

    public float startAttackTime;
    public float punchRange = 0.5f;
    public float kickRange = 0.5f;
    public int punchDamage = 40;
    public int kickDamage = 40;


    //Assigning a variable where we'll store the Rigidbody2D component.
    private Rigidbody2D rb;

    private float currentDodgeTime;
    private float currentAttackTime;
    private bool isDodging;
    private bool isAttacking;
    private bool isInvincible;
    private bool isDodgeCoolingDown;
    private bool isAttackCoolingDown;

    [SerializeField]
    private float invincibilityDurationSeconds;
    [SerializeField]
    private float dodgeCooldownSeconds;
    [SerializeField]
    private float attackCooldownSeconds;

    // Start is called before the first frame update
    void Start()
    {
        //Sets our variable 'rb' to the Rigidbody2D component on the game object where this script is attached.
        rb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        //Combat shit
        {

            if (Input.GetKeyDown(KeyCode.W) && isDodging == false && isAttackCoolingDown == false)
            {
                isAttacking = true;
                currentAttackTime = startAttackTime;

                //kick off the coroutine and punch method
                StartCoroutine(AttackCooldown());
                Punch();
            }

            if (Input.GetKeyDown(KeyCode.D) && isDodging == false && isAttackCoolingDown == false)
            {
                isAttacking = true;
                currentAttackTime = startAttackTime;

                //kick off the coroutine and kick method
                StartCoroutine(AttackCooldown());
                Kick();
            }
        }

        //Movement code for left and right arrow keys.
        if (Input.GetKey(KeyCode.LeftArrow) && isAttacking == false && isDodging == false)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            if (playerSprite != null)
            {
                //playerSprite.flipX = true;
            }
        }

        else if (Input.GetKey(KeyCode.RightArrow) && isAttacking == false && isDodging == false)
        {
            rb.velocity = new Vector2(+moveSpeed, rb.velocity.y);
            if (playerSprite != null)
            {
                //playerSprite.flipX = false;
            }
            else
            {
                Debug.Log("Sprite Renderer not found");
            }
        }
        //ELSE if we're not pressing an arrow key, our velocity is 0 along the X axis, and whatever the Y velocity is (determined by jump)
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
         
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("speed", rb.velocity.magnitude);
        }

        if(Input.GetKeyDown(KeyCode.A) && isAttacking == false && isDodgeCoolingDown == false)
        {
            //start the dodge coroutine
            StartCoroutine(DodgeCooldown());
            playerAnimator.SetTrigger("dodge");

            isDodging = true;
            currentDodgeTime = startDodgeTime;
        }

        //Checks to see if dodging; if so, add dodge force for amount of time and
        //begin dodge cooldown period
        if (isDodging)
        {
            rb.velocity = transform.right * -dodgeForce;
            currentDodgeTime -= Time.deltaTime;

            //start the invincibility coroutine
            StartCoroutine(BecomeTemporarilyInvincible());

           if (currentDodgeTime <= 0)
            {
                isDodging = false;
            }
        }

        if (isAttacking)
        {
            currentAttackTime -= Time.deltaTime;
            if (currentAttackTime <= 0)
            {
                isAttacking = false;
            }
        }

       


    }


 private void OnCollisionStay2D(Collision2D collision)
    {
        //If we collide with an object tagged "ground" then our jump resets and we can now jump.
        //if (collision.gameObject.tag == "ground")
        {

        }
    }

    void Punch()
    {
        //play attack animation
        playerAnimator.SetTrigger("punch");

        //detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(punchPoint.position, punchRange, enemyLayers);

        //damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().EnemyTakeDamage(punchDamage);
            Debug.Log("We punched " + enemy.name);
        }



    }

    void Kick()
    {
        //play attack animation
        playerAnimator.SetTrigger("kick");

        //detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(kickPoint.position, kickRange, enemyLayers);

        //damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().EnemyTakeDamage(kickDamage);
            Debug.Log("We kicked " + enemy.name);
        }


    }

    void onDrawGizmosSelected()
    {
        if (punchPoint == null)
            return;

        if (kickPoint == null)
            return;

        Gizmos.DrawWireSphere(punchPoint.position, punchRange);
        Gizmos.DrawWireSphere(kickPoint.position, kickRange);
        Gizmos.color = Color.white;
    }

    private IEnumerator BecomeTemporarilyInvincible()
    {
        //kicks off cooldown time
        Debug.Log("Player is dodging!");
        isInvincible = true;

        yield return new WaitForSeconds(invincibilityDurationSeconds);

        //ends cooldown time
        isInvincible = false;
        Debug.Log("Player is no longer dodging");
    }

    private IEnumerator DodgeCooldown()
    {
        //kicks off cooldown time
        Debug.Log("Dodge cooldown started");
        isDodgeCoolingDown = true;

        yield return new WaitForSeconds(dodgeCooldownSeconds);

        //ends cooldown time
        isDodgeCoolingDown = false;
        Debug.Log("Dodge cooldown finished");
    }

    private IEnumerator AttackCooldown()
    {
        //kicks off cooldown time
        Debug.Log("Attack cooldown started");
        isAttackCoolingDown = true;

        yield return new WaitForSeconds(attackCooldownSeconds);

        //ends cooldown time
        isAttackCoolingDown = false;
        Debug.Log("Attack cooldown finished");
    }

}


