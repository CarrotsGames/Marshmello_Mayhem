using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public bool isAlive;
    public int maxHealth = 10;
    public int currentHealth;
    public GameObject respawnPoint;
    private GameController gameController;

    public float respawnTimer;

    private float timeBetweenHeals;
    private int healValue;
    private float timer;

    // Use this for initialization
    void Start()
    {
        currentHealth = maxHealth;
        isAlive = true;

        if (respawnPoint == null)
        {
            Debug.Log("Player does not have respawn point");
        }

        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        timeBetweenHeals = gameController.timeBetweenPlayerHeals;
        healValue = gameController.healthGain;
        
        if (isAlive == false)
        {
            currentHealth = 0;
        }
        if (currentHealth <= 0 && isAlive == true)
        {
            Death();
        }

        timer += Time.deltaTime;

        if (timer >= timeBetweenHeals)
        {
            Heal(healValue);

            timer = 0;
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

    public void Heal(int a_healValue)
    {
        currentHealth += a_healValue;
    }
}
