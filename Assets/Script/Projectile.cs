using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public LayerMask collisionMask;
	float speed = 10;
	float damage = 1;
	float lifetime = 3;
	float skinwidth = 0.1f;

	public void Start () {
		Destroy(gameObject, lifetime);

		Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
		if(initialCollisions.Length > 0) {
			OnHitObject(initialCollisions[0]);
		}
	}

	public void SetSpeed(float speed) {
		this.speed = speed;
	}
	
	// Update is called once per frame
	void Update () {
		float moveDistance = speed * Time.deltaTime;
		CheckCollisions (moveDistance);
		transform.Translate (Vector3.forward * Time.deltaTime * speed);
	}

	void CheckCollisions (float moveDistance) {
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, moveDistance + skinwidth, collisionMask, QueryTriggerInteraction.Collide)) {
			OnHitObject(hit);
		}
	}

	void OnHitObject (RaycastHit hit) {
		IDamgeable damageableObject = hit.collider.GetComponent<IDamgeable> ();
		if (damageableObject != null) {
			damageableObject.TakeHit (damage, hit);
		}
		GameObject.Destroy (gameObject);
	}

	void OnHitObject (Collider c) {
		IDamgeable damageableObject = c.GetComponent<IDamgeable> ();
		if (damageableObject != null) {
			damageableObject.TakeDamage (damage);
		}
		GameObject.Destroy (gameObject);
	}
}
