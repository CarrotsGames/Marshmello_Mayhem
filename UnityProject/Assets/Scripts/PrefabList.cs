using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PrefabList : MonoBehaviour {

    public GameObject Pit;
    public GameObject PlaceableWall;
    public GameObject HumanThrower;
    public GameObject LastPrayer;

    public AudioSource BuildingAudio;
    public AudioSource ExplosionAudio;
    public AudioSource ShootingAudio;
    public AudioSource PitAudio;
    public AudioSource PlayerDeathAudio;
    public AudioSource GameOverAudio;

    public GameObject blueTeamRespawnParticles;
    public GameObject redTeamRespawnParticles;
    public GameObject deathParticles;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
