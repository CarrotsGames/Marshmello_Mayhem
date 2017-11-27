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
    private PlayerController playerController;
    private PrefabList prefabList;

    public float respawnTimer;
    private GameObject respawnParticles;
    private GameObject deathParticles;

    private float timeBetweenHeals;
    private int healValue;
    private float timer;

    private float combatTimer;
    private float timeOutOfCombat;
    bool isInCombat = false;

    public bool isChangingColour;
    float colourTime = 0.0f;
    public float timeSpentAsDifferentColour;
    public Material emissiveMaterial;

    BuildTrap playerBuildMode;
    Transform deathPosition;
    //Renderer renderer;

    // Use this for initialization
    void Start()
    {
        
        playerBuildMode = GetComponent<BuildTrap>();

        currentHealth = maxHealth;
        isAlive = true;

        playerController = GetComponent<PlayerController>();
        prefabList = FindObjectOfType<PrefabList>();
        gameController = FindObjectOfType<GameController>();

        if (playerController.teamNumber == 1)
        {
            respawnParticles = Instantiate<GameObject>(prefabList.blueTeamRespawnParticles, respawnPoint.transform);
        }
        if (playerController.teamNumber == 2)
        {
            respawnParticles = Instantiate<GameObject>(prefabList.redTeamRespawnParticles, respawnPoint.transform);
        }

        respawnParticles.transform.position = respawnPoint.transform.position;
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
            deathPosition = transform;
            Death();
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

        GetComponentInChildren<ColourChange>().isChanging = true;
        
        //when player takes damage, set isInCombat to true
        isInCombat = true;
    }

    public void Death()
    {
        deathParticles = Instantiate<GameObject>(prefabList.deathParticles, deathPosition);
        deathParticles.transform.SetParent(null);
        deathParticles.GetComponent<ParticleSystem>().Play();

        Invoke("DisableDeathParticles", 1.0f);

        playerBuildMode.canBuild = false;
        playerBuildMode.isEnabled = false;
        //plays sound
        if (FindObjectOfType<PrefabList>().PlayerDeathAudio != null)
        {
            FindObjectOfType<PrefabList>().PlayerDeathAudio.Play();
        }

        playerController.DropChemFlag();
        currentHealth = 0;
        isAlive = false;
        transform.position = new Vector3(0, 0, 0);
        Invoke("ActivateParticles", respawnTimer - 0.5f);
        Invoke("Respawn", respawnTimer);
    }

    private void Respawn()
    {              
        Invoke("DisableParticles", 0.5f);

        //stops sound
        if (FindObjectOfType<PrefabList>().PlayerDeathAudio != null)
        {
            FindObjectOfType<PrefabList>().PlayerDeathAudio.Stop();
        }

        isAlive = true;
        playerController.transform.position = respawnPoint.transform.position;
        currentHealth = maxHealth;
        playerController.playerMovement = true;
        playerBuildMode.canBuild = true;

        Destroy(deathParticles);
    }

    public void Heal(int a_healValue)
    {
        currentHealth += a_healValue;
    }

    private void ActivateParticles()
    {
        respawnParticles.GetComponent<ParticleSystem>().Play();
    }

    private void DisableParticles()
    {
        respawnParticles.GetComponent<ParticleSystem>().Stop();
    }

    private void DisableDeathParticles()
    {
        deathParticles.GetComponent<ParticleSystem>().Stop();
    }
}
