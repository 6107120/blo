using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {

	Rigidbody myRigidBody;
	Vector3 velocity;
	// Use this for initialization
	void Start () {
		myRigidBody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	
	
	public void Move(Vector3 velocity) {
		this.velocity = velocity;
	}

	public void LookAt (Vector3 LookAt) {
		Vector3 heightCorrectedPoint = new Vector3 (LookAt.x, transform.position.y ,LookAt.z);
		transform.LookAt (heightCorrectedPoint);
		
	}
	 void FixedUpdate() {
		myRigidBody.MovePosition (myRigidBody.position + velocity * Time.fixedDeltaTime);
	}
}
