using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public bool isEnabled;
    public bool display;
    public GameObject projectile;
    private float projectileSpeed;
    private float timeBetweenShots;
    private float bulletCost;

    public Transform firePoint;

	// Use this for initialization
	void Start ()
    {
        isEnabled = true;
        display = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        bulletCost = FindObjectOfType<GameController>().shootingCost;

        if (FindObjectOfType<GameController>() != null)
        {
            timeBetweenShots = FindObjectOfType<GameController>().projectileCooldown;
            projectileSpeed = FindObjectOfType<GameController>().projectileSpeed;
        }

        if (display == true)
        {
            //display gun mesh
            gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
        }

        if (display == false)
        {
            //hide gun mesh
            gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        }
    }

    public void Shoot()
    {
        if (isEnabled == true && display == true)
        {
            GetComponentInParent<ResourceController>().currentResource -= bulletCost;

            //create new projectile with force in direction
            GameObject newProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation) as GameObject;
            newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * projectileSpeed, ForceMode.Impulse);
            isEnabled = false;

            //plays audio
            if (FindObjectOfType<PrefabList>().ShootingAudio != null)
            {
                FindObjectOfType<PrefabList>().ShootingAudio.Play();
            }

            //call delay after timeBetweenShots
            Invoke("Delay", timeBetweenShots);            
        } 
    }

    void Delay()
    {
        //stops audio
        if (FindObjectOfType<PrefabList>().ShootingAudio != null)
        {
            FindObjectOfType<PrefabList>().ShootingAudio.Stop();
        }
        Invoke("GunDisplayDelay", timeBetweenShots);
        isEnabled = true;
    }

    void GunDisplayDelay()
    {
        display = false;
        GetComponentInParent<PlayerController>().isShooting = false;
    }
}
