using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    int damage;
    float timer;
    float cooldown;
    bool onCooldown;

    GameController gameController;

	// Use this for initialization
	void Start () {

        if (FindObjectOfType<GameController>() != null)
        {
            gameController = FindObjectOfType<GameController>();

            damage = gameController.gunDamage;
            timer = gameController.projectileTimer;
            cooldown = gameController.projectileCooldown;            
        }
        else
        {
            Debug.Log("No object with GameController script");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer -= Time.deltaTime;

		if (timer <= 0)
        {
            Destroy(gameObject);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlaceableWall>() != null)
        {
            other.GetComponent<PlaceableWall>().TakeDamage(damage);
            Destroy(gameObject);
        }

        if (other.GetComponent<PlayerHealth>() != null)
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
