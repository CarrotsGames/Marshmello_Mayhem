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

    private float combatTimer;
    private float timeOutOfCombat;
    bool isInCombat = false;

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
        //set values to common variables in gameController
        timeBetweenHeals = gameController.timeBetweenPlayerHeals;
        healValue = gameController.healthGain;
        timeOutOfCombat = gameController.timeOutOfCombat;

        if (isAlive == false)
        {
            currentHealth = 0;
        }
        if (currentHealth <= 0 && isAlive == true)
        {
            Death();
        }

        //check if player has taken damage
        if (isInCombat == false)
        {
            timer += Time.deltaTime;

            if (timer >= timeBetweenHeals)
            {
                Heal(healValue);

                timer = 0;
            }
        }
        //prevent currentHealth from exceeding maxHealth
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }

        //checks if player has taken damage
        if (isInCombat == true)
        {
            combatTimer += Time.deltaTime;

            if (combatTimer >= timeOutOfCombat)
            {
                isInCombat = false;

                combatTimer = 0;
                timer = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            TakeDamage(maxHealth);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmount;
        }

        //when player takes damage, set isInCombat to true
        isInCombat = true;
    }

    public void Death()
    {
        //plays sound
        if (FindObjectOfType<PrefabList>().PlayerDeathAudio != null)
        {
            FindObjectOfType<PrefabList>().PlayerDeathAudio.Play();
        }

        GetComponent<PlayerController>().DropChemFlag();
        currentHealth = 0;
        isAlive = false;
        transform.position = new Vector3(0, 0, 0);
        Invoke("Respawn", respawnTimer);
    }

    private void Respawn()
    {
        //stops sound
        if (FindObjectOfType<PrefabList>().PlayerDeathAudio != null)
        {
            FindObjectOfType<PrefabList>().PlayerDeathAudio.Stop();
        }

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
