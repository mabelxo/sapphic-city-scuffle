using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int maxEnemyHealth = 100;
    int currentEnemyHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentEnemyHealth = maxEnemyHealth;
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

}
