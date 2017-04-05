using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

	public Transform weaponHold;
	public Gun startingGun;
	Gun equippedGun;

	void Start() {
		if(startingGun != null) {
			EquipGun(startingGun);
		}
	}

	public void EquipGun(Gun gun) {
		if(equippedGun != null) {
			Destroy(equippedGun.gameObject);
		}
		equippedGun = Instantiate (gun, weaponHold.position, weaponHold.rotation);
		equippedGun.transform.parent = weaponHold;
	}

	public void Shoot() {
		if(equippedGun != null) {
			equippedGun.Shoot();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
