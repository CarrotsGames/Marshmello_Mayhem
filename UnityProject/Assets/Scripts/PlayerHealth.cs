using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public int startingHealth = 10;
    public int currentHealth;
    public Slider HealthSlider;
    bool isDead;
    bool damaged;

    // Use this for initialization
    void Start()
    {
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (damaged)
        {

        }

        damaged = false;
    }

    public void TakeDamage(int amount)
    {
        damaged = true;
        if (!isDead)
        {
            currentHealth -= amount;
            //HealthSlider.value = currentHealth - attackDamage;
        }
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        //playerMovement.enabled = false;

    }
}
