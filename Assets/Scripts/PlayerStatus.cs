using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    //Set up health variables n shit
    public int maxHealth = 100;
    private int currentHealth;
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>(); 
        currentHealth = maxHealth;
    }

    public void PlayerTakeDamage(int damage)
    {
        if (playerController.isInvincible == false)
        {
            currentHealth -= damage;
            Debug.Log("Player took damage");
        }

        //play hurt animation

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {

        //Die animation

        //Disable enemy
        Debug.Log("Player died");
    }
}
