using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public bool isEnabled;

    public GameObject projectile;
    public float projectileSpeed;
    private float timeBetweenShots;
    
    public int damage;

    public Transform firePoint;

	// Use this for initialization
	void Start ()
    {
		if (FindObjectOfType<GameController>() != null)
        {
            timeBetweenShots = FindObjectOfType<GameController>().projectileCooldown;
        }
        else
        {
            Debug.Log("No object with GameController script");
        }
        isEnabled = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    public void Shoot()
    {
        if (isEnabled == true)
        {
            isEnabled = false;

            //create new projectile with force in direction
            GameObject newProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation) as GameObject;
            newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * projectileSpeed, ForceMode.Impulse);
            //call delay after timeBetweenShots
            Invoke("Delay", timeBetweenShots);
        }        
    }

    void Delay()
    {
        isEnabled = true;
    }
}
