using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //Public combat variables
    public Transform enemyPunchPoint;
    public Transform enemyKickPoint;
    public LayerMask playerLayers;
    public Animator enemyAnimator;
    public SpriteRenderer enemySprite;

    public float enemyMoveSpeed;
    public float enemyDodgeForce;
    public float enemyStartDodgeTime;
    public float startEnemyAttackTime;
    public float enemyPunchRange = 0.5f;
    public float enemyKickRange = 0.5f;
    public int enemyPunchDamage = 40;
    public int enemyKickDamage = 40;

    //Health
    public int maxEnemyHealth = 100;
    int currentEnemyHealth;

    //Other shit, private n shit
    private float currentEnemyDodgeTime;
    private float currentEnemyAttackTime;
    private bool isEnemyDodging;
    private bool isEnemyAttacking;
    private bool isEnemyInvincible;
    private bool isEnemyDodgeCoolingDown;
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
        currentEnemyHealth = maxEnemyHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && isEnemyDodging == false && isAttackCoolingDown == false)
        {
            isEnemyAttacking = true;
            currentEnemyAttackTime = startEnemyAttackTime;

            //kick off the coroutine and punch method
            StartCoroutine(AttackCooldown());
            Punch();
        }

        if (Input.GetKeyDown(KeyCode.J) && isEnemyDodging == false && isAttackCoolingDown == false)
        {
            isEnemyAttacking = true;
            currentEnemyAttackTime = startEnemyAttackTime;

            //kick off the coroutine and kick method
            StartCoroutine(AttackCooldown());
            Kick();
        }


    }

    public void EnemyTakeDamage(int damage)
    {
        currentEnemyHealth -= damage;

        //play hurt animation

        if(currentEnemyHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {

        //Die animation

        //Disable enemy
        Debug.Log("Enemy died");
    }

    void Punch()
    {
        //play attack animation
        enemyAnimator.SetTrigger("punch");

        //detect enemies in range of attack
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(enemyPunchPoint.position, enemyPunchRange, playerLayers);

        //damage them
        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<PlayerStatus>().PlayerTakeDamage(enemyPunchDamage);
            Debug.Log("Enemy punched player!");
        }



    }

    void Kick()
    {
        //play attack animation
        enemyAnimator.SetTrigger("kick");

        //detect enemies in range of attack
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(enemyKickPoint.position, enemyKickRange, playerLayers);

        //damage them
        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<PlayerStatus>().PlayerTakeDamage(enemyPunchDamage);
            Debug.Log("Enemy kicked player!");
        }


    }

    private IEnumerator AttackCooldown()
    {
        //kicks off cooldown time
        Debug.Log("Enemy attack cooldown started");
        isAttackCoolingDown = true;

        yield return new WaitForSeconds(attackCooldownSeconds);

        //ends cooldown time
        isAttackCoolingDown = false;
        Debug.Log("Enemy attack cooldown finished");
    }

}
