using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public bool isFiring;

    public GameObject projectile;
    public float projectileSpeed;
    public float timeBetweenShots;

    private float projectileCount;

    public Transform firePoint;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(isFiring)
        {
            projectileCount -= Time.deltaTime;
            if (projectileCount <= 0)
            {
                projectileCount = timeBetweenShots;
                GameObject newProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation) as GameObject;
                //newProjectile.speed = projectileSpeed;
                newProjectile.GetComponent<Rigidbody>().AddForce (newProjectile.transform.forward * projectileSpeed, ForceMode.Impulse);
            }
        }
        else
        {
            projectileCount = 0;
        }
	}
}
