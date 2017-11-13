using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invention_007_LastPrayer : TrapBehaviour {

    public float countdown;
    public float explosionRadius;
    List<GameObject> traps;
    public int explosionDamage;
    GameObject[] players;
    bool hasFinishedPlaying = false;
    AudioSource explosionSound;    

	// Use this for initialization
	void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        explosionSound = FindObjectOfType<PrefabList>().ExplosionAudio;
        trapName = "LastPrayer";
	}
	
	// Update is called once per frame
	void Update () {
        traps = FindObjectOfType<GameController>().placedTraps;

        countdown -= Time.deltaTime;

        if (!hasFinishedPlaying)
        {
            if (countdown <= 0)
            {
                Explosion();

                traps.Remove(gameObject);

                gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;

                

                hasFinishedPlaying = true;
            }
        }
        if (explosionSound.isPlaying == false && hasFinishedPlaying == true)
        {
            Debug.Log("Destroy");
            Destroy(gameObject);
        }
    }

    void Explosion()
    {
        //play game over sound
        if (explosionSound != null)
        {
            Debug.Log("Play audio");
            explosionSound.Play();
        }

        for (int i = 0; i < traps.Count; i++)
        {
            Vector3 vecBetween = transform.position - traps[i].transform.position;
            vecBetween.y = 0;

            if (vecBetween.magnitude <= explosionRadius)
            {
                if (traps[i].GetComponent<PlaceableWall>() != null)
                {                    
                    traps[i].GetComponent<PlaceableWall>().TakeDamage(explosionDamage);
                }                

                if (traps[i].GetComponent<Invention_005_PillarOfSaws>() != null)
                {
                    traps[i].GetComponent<Invention_005_PillarOfSaws>().TakeDamage(explosionDamage);
                }
            }
        }

        for (int i = 0; i < players.Length; i++)
        {
            Vector3 vecBetween = transform.position - players[i].transform.position;
            vecBetween.y = 0;

            if (vecBetween.magnitude <= explosionRadius)
            {
                if (players[i].GetComponent<PlayerHealth>() != null)
                {
                    players[i].GetComponent<PlayerHealth>().TakeDamage(explosionDamage);
                }
            }
        }
    }

}
