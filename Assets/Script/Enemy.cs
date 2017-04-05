using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class Enemy : LivingEntity {

	public enum State {Idle, Chasing, Attacking};
	State currnetState;
	NavMeshAgent pathfinder;
	Transform target;
	Material skinMaterial;
	LivingEntity targetEntity;

	Color originalColor;

	float attackDistanceThreshold = 0.5f;
	float timeBetweenAttacks = 1;
	float damage = 1;

	float nextAttactTime;
	float myCollisionRadius;
	float targetCollisionRadius;
	bool hasTarget;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		pathfinder = GetComponent<NavMeshAgent> ();
		skinMaterial = GetComponent<Renderer> ().material;
		originalColor = skinMaterial.color;

		if(GameObject.FindGameObjectWithTag("Player") != null) {
			currnetState = State.Chasing;
			hasTarget = true;

			target = GameObject.FindGameObjectWithTag("Player").transform;
			targetEntity = target.GetComponent<LivingEntity> ();
			targetEntity.OnDeath += onTargetDeath;

			myCollisionRadius = GetComponent<CapsuleCollider> ().radius;
			targetCollisionRadius = target.GetComponent<CapsuleCollider> ().radius;
			StartCoroutine(UpdatePath ());
		}
	}

	void onTargetDeath () {
		hasTarget = false;
		currnetState = State.Idle;
	}
	
	// Update is called once per frame
	void Update () {
		if(hasTarget){
			if(Time.time > nextAttactTime) {
				float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
				if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius,2)) {
					nextAttactTime = Time.time + timeBetweenAttacks;
					StartCoroutine(Attack());
				}
			}
		}
	}
	IEnumerator Attack() {
		currnetState = State.Attacking;
		pathfinder.enabled = false;

		Vector3 originalPosition = transform.position;
		Vector3 dirToTarget = (target.position - transform.position).normalized;
		Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

		float percent = 0;
		float attackSpeed = 3;

		skinMaterial.color = Color.red;
		bool hasAppliedDamage = false;

		while (percent <= 1) {
			if(percent >= .5f && !hasAppliedDamage) {
				hasAppliedDamage = true;
				targetEntity.TakeDamage(damage);
			}

			percent += Time.deltaTime * attackSpeed;
			float interpolation = (-percent * percent + percent) * 4;
			transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
			yield return null;
		}
		skinMaterial.color = originalColor;
		currnetState = State.Chasing;
		pathfinder.enabled = true;
	}
	IEnumerator UpdatePath() {
		float refreshRate = 0.25f;
		while (hasTarget) {
			if(currnetState == State.Chasing){
				Vector3 dirToTarget = (target.position - transform.position).normalized;
				Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
				if (!dead) {
					pathfinder.SetDestination(targetPosition);
				}
			}
			yield return new WaitForSeconds(refreshRate);
		}
	}
}
