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

    // Use this for initialization
    void Start()
    {
        
        currentHealth = maxHealth;
        isAlive = true;

        if (respawnPoint == null)
        {
            Debug.Log("Player does not have respawn point");
        }
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
        if (isChangingColour == true)
        {
            //Renderer renderer = new Renderer();
            //Material mat = renderer.material;
            //
            //float emission = Mathf.PingPong(Time.time, 1.0f);
            //Color baseColor = Color.red;
            //
            //Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
            //
            //mat.SetColor(11208, finalColor);
            //
            colourTime += Time.deltaTime;
                        
            if (colourTime >= 0.5f)
            {
                //GetComponentInChildren<Material>().SetColor(11208, Color.black);
                //GetComponentInChildren<Renderer>().material.color = Color.red;
                //GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", Color.red * 1.0f);
                isChangingColour = false;

                colourTime = 0;
            }
        }
        if (isChangingColour == false)
        {
            //GetComponentInChildren<Renderer>().material.color = Color.red;
            //GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", Color.red * 0);
        }

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

        isChangingColour = true;

        //when player takes damage, set isInCombat to true
        isInCombat = true;
    }

    public void Death()
    {
        Transform ghost = transform;

        //deathParticles = Instantiate<GameObject>(prefabList.deathParticles, ghost);

        //deathParticles.GetComponent<ParticleSystem>().Play();

        //Invoke("DisableDeathParticles", 1.0f);

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
        //deathParticles.GetComponent<ParticleSystem>().Stop();
    }
}
