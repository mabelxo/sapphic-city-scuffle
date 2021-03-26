using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public Animator playerAnimator;
    public Transform punchPoint;
    public Transform kickPoint;
    public float punchRange = 0.5f;
    public float kickRange = 0.5f;
    public LayerMask enemyLayers;

    public int punchDamage = 40;
    public int kickDamage = 40;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            Punch();
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            Kick();
        }
    }

    void Punch()
    {
        //play attack animation
        playerAnimator.SetTrigger("punch");

        //detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(punchPoint.position, punchRange, enemyLayers);

        //damage them
        foreach(Collider2D enemy in hitEnemies)
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

}
