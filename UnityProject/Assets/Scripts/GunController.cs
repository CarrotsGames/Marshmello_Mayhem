using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public bool isFiring;

    public GameObject projectile;
    public float projectileSpeed;
    public float timeBetweenShots;
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
		if(isFiring)
        {
            projectileCount -= Time.deltaTime;
            if (projectileCount <= 0)
            {
                projectileCount = timeBetweenShots;
                GameObject newProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation) as GameObject;
                newProjectile.GetComponent<Rigidbody>().AddForce (newProjectile.transform.forward * projectileSpeed, ForceMode.Impulse);
                projectileTimer += 1.0f * Time.deltaTime;
                
                if (projectileTimer >= 4)
                {
                    DestroyObject(projectile.gameObject);
                }
            }
        }
        else
        {
            projectileCount = 0;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (projectile.gameObject.tag == "Wall")
        {
            DestroyObject(projectile.gameObject);
        }
    }
}
