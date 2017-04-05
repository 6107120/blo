using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamgeable {

	void TakeHit (float damge, RaycastHit hit);	
	void TakeDamage (float damage);	
		
}

