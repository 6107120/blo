﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(KSPlayerController))]
public class KSPlayer : MonoBehaviour {

	private Transform pickedUpBlock;
	private Vector3 lastPosition;
	public float moveSpeed = 5;
	KSPlayerController playerController;
	public LayerMask layerMask;

	// Use this for initialization
	void Start () {
		playerController = GetComponent<KSPlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		//rotation
		if(Input.GetKey(KeyCode.LeftArrow)){
			if(transform.forward != Vector3.left)
			playerController.Rotation(90);
		}
		else if(Input.GetKey(KeyCode.RightArrow)){
			if(transform.forward != Vector3.right)
			playerController.Rotation(90);
		}
		else if(Input.GetKey(KeyCode.UpArrow)){
			if(transform.forward != Vector3.forward)
			playerController.Rotation(90);
		}
		else if(Input.GetKey(KeyCode.DownArrow)){
			if(transform.forward != Vector3.back)
			playerController.Rotation(90);
		}
		
		//move
		Vector3 moveInput = Vector3.zero;
		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)){
			moveInput = new Vector3(0, 0, Input.GetAxisRaw("Vertical"));
		}
		else if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)){
			moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
		}
		Vector3 moveVelocity = moveInput.normalized * moveSpeed;
		playerController.Move(moveVelocity);

		//ray
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;
		Debug.DrawRay(ray.origin, ray.direction, Color.red);

		if(Input.GetKeyDown(KeyCode.F)){
			if(Physics.Raycast(ray,out hit, 1f, layerMask)) {
				pickedUpBlock = hit.transform;
			}
			pickedUpBlock.transform.position = pickedUpBlock.transform.position + transform.forward;
			pickedUpBlock = null;
			transform.position = transform.position + transform.forward;
		}
		if(Input.GetKeyDown(KeyCode.D)){
			if(Physics.Raycast(ray,out hit, 1f, layerMask)) {
				pickedUpBlock = hit.transform;
			}
		}
		if(pickedUpBlock) {
			pickedUpBlock.transform.position = pickedUpBlock.transform.position - transform.forward;
			pickedUpBlock = null;
			transform.position = transform.position - transform.forward;
		}
		//lastPosition = transform.position;
		
	}
}