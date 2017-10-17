using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chem_Flag : MonoBehaviour {

	public bool isBeingCarried = false;
	public int teamNumber = 1;
	//(blue = 1, red = 2);

//	void OnTriggerStay(Collider Other){
//
//		if (isBeingCarried) {
//			return;
//		}
//
//		if (Other.tag != "Player") {
//			return;
//		}
//			
//		PlayerController playerController = Other.GetComponent<PlayerController> ();
//
//		if (playerController.teamNumber != teamNumber) {
//			playerController.CanPickUpChemFlag ();
//		}
//	}

	public void PickUpChemFlag(){
		//playerController.PickUpChemFlag (this);
		GetComponent<BoxCollider> ().enabled = false;
		isBeingCarried = true;
	}

	public void DropChemFlag(){
		isBeingCarried = false;
		GetComponent<BoxCollider> ().enabled = true;
		transform.SetParent (null);
	}
}
