using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public bool isAlive;
    public int maxHealth = 10;
    public int currentHealth;
    public GameObject respawnPoint;

    public float respawnTimer;

    // Use this for initialization
    void Start()
    {
        currentHealth = maxHealth;
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive == false)
        {
            currentHealth = 0;
        }
        if (currentHealth <= 0 && isAlive == true)
        {
            Death();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmount;
        }
    }

    public void Death()
    {
        GetComponent<PlayerController>().DropChemFlag();
        currentHealth = 0;
        isAlive = false;
        Invoke("Respawn", respawnTimer);
    }

    private void Respawn()
    {
        isAlive = true;
        GetComponent<PlayerController>().transform.position = respawnPoint.transform.position;
        currentHealth = maxHealth;
        GetComponent<PlayerController>().playerMovement = true;
    }
}
