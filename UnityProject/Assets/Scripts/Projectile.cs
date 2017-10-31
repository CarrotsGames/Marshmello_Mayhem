using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    int damage;
    float timer;
    float cooldown;
    float pushback;

    GameController gameController;

	// Use this for initialization
	void Start () {

        if (FindObjectOfType<GameController>() != null)
        {
            gameController = FindObjectOfType<GameController>();

            damage = gameController.gunDamage;
            timer = gameController.projectileTimer;
            cooldown = gameController.projectileCooldown;

            pushback = gameController.projectilePushback;
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
        if (other.gameObject.GetComponent<PlaceableWall>() != null)
        {            
            other.gameObject.GetComponent<PlaceableWall>().TakeDamage(damage);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Player")
        {
            GetComponent<Collider>().enabled = false;

            //pushback
            other.gameObject.transform.position += transform.forward * gameController.projectilePushback;

            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "LevelWall")
        {
            Destroy(gameObject);
        }
    }

}
