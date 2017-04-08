using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class KSPlayerController : MonoBehaviour {

	Vector3 velocity;
	float rotation;
	Rigidbody myRigidbody;

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody> ();
		
	}
	
	public void Move(Vector3 velocity) {
		this.velocity = velocity;
	}
	public void Rotation(float rotation) {
		this.rotation = rotation;
	}

	public void FixedUpdate() {
		//rotation
		Quaternion newRotation = transform.rotation * Quaternion.Euler(0,rotation,0);
		rotation = 0;
		myRigidbody.MoveRotation(newRotation);
		//move
		myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
	}
}
