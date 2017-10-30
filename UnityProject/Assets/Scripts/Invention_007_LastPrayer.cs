using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invention_007_LastPrayer : TrapBehaviour {

    float countdown;
    float explosionRadius = 10.0f;
    List<GameObject> traps;
    int explosionDamage = 100;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        countdown -= Time.deltaTime;

        if (countdown <= 0)
        {
            Explosion();
        }
	}

    void Explosion()
    {
        traps = FindObjectOfType<GameController>().placedTraps;

        for (int i = 0; i < traps.Count; i++)
        {
            if (traps[i].GetComponent<PlaceableWall>() != null && traps[i].GetComponent<Invention_005_PillarOfSaws>() != null)
            {
                Vector3 vecBetween = transform.position - traps[i].transform.position;
                vecBetween.y = 0;

                if (vecBetween.magnitude <= explosionRadius)
                {
                    traps[i].GetComponent<PlaceableWall>().TakeDamage(explosionDamage);
                    traps[i].GetComponent<Invention_005_PillarOfSaws>().TakeDamage(explosionDamage);
                }

            }
        }
    }
}
