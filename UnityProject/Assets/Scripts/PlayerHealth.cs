using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public bool isAlive;
    public int startingHealth = 10;
    public int currentHealth;
    private Vector3 respawnPoint;

    public Vector3 team1Respawn;
    public Vector3 team2Respawn;

    public float respawnDelay;

    // Use this for initialization
    void Start()
    {
        currentHealth = startingHealth;

        if (GetComponent<PlayerController>().teamNumber == 1)
        {
            respawnPoint = team1Respawn;

        }
        if (GetComponent<PlayerController>().teamNumber == 2)
        {
            respawnPoint = team2Respawn;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Death();
        }
        else
        {
            isAlive = true;
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
        isAlive = false;
        Invoke("Respawn", respawnDelay);
    }

    private void Respawn()
    {
        GetComponent<PlayerController>().transform.position = respawnPoint;
    }
}
