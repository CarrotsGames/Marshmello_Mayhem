using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public int startingHealth = 10;
    public int currentHealth;

    // Use this for initialization
    void Start()
    {
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0)
        {
            currentHealth -= damageAmount;
        }
    }

    void Death()
    {
        gameObject.SetActive(false);
        //playerMovement.enabled = false;

    }
}
