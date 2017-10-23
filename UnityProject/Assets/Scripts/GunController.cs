using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public bool isEnabled;

    public GameObject projectile;
    public float projectileSpeed;
    public float timeBetweenShots = 2.0f;
    public float projectileTimer;
    private float projectileCount;

    public Transform firePoint;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    private void OnTriggerEnter(Collider other)
    {
        if (projectile.gameObject.tag == "Wall" || projectile.gameObject.tag == "Player")
        {
            DestroyObject(projectile.gameObject);
        }
    }

    void Delay()
    {
        isEnabled = true;
    }

    public void Shoot()
    {
        if (isEnabled == true)
        {
            isEnabled = false;

            //create new projectile with force in direction
            GameObject newProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation) as GameObject;
            newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * projectileSpeed, ForceMode.Impulse);            
        }

        //call delay after timeBetweenShots
        Invoke("Delay", timeBetweenShots);
    }

    //public void Expire()
    //{
    //    Destroy(projectile.gameObject);
    //}
}
