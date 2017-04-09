using System.Collections;
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
		
		//cal ray
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;
		Debug.DrawRay(ray.origin, ray.direction, Color.gray);
		Ray upBaseray = new Ray(transform.position - Vector3.up * transform.localScale.x, transform.forward);
		Ray upForwardRay = new Ray(transform.position + Vector3.up, transform.forward);

		Ray downRayLeftForward = new Ray(transform.position + Vector3.left/2 + Vector3.forward/2, Vector3.down);
		Ray downRayLeftBack = new Ray(transform.position + Vector3.left/2 + Vector3.back/2, Vector3.down);
		Ray downRayRightForward = new Ray(transform.position + Vector3.right/2 + Vector3.forward/2, Vector3.down);
		Ray downRayRightBack = new Ray(transform.position + Vector3.right/2 + Vector3.back/2, Vector3.down);
		// Debug.DrawRay(upBaseray.origin, upBaseray.direction, Color.red);
		// Debug.DrawRay(upRay.origin, upRay.direction, Color.red);
		// Debug.DrawRay(upForwardRay.origin, upForwardRay.direction, Color.red);

		// Debug.DrawRay(ray.origin, ray.direction, Color.green);
		// Debug.DrawRay(downForwardRay.origin, downForwardRay.direction, Color.yellow);
		Debug.DrawRay(downRayLeftForward.origin, downRayLeftForward.direction, Color.yellow);	

		if(Physics.Raycast(upBaseray, 0.5f, layerMask)) {
				if(Physics.Raycast(upForwardRay, 1f, layerMask) == false) {
					moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 1.5f, Input.GetAxisRaw("Vertical"));
				}
		}else
				if(Physics.Raycast(downRayLeftForward, 0.5f, layerMask) == false
				&& Physics.Raycast(downRayLeftBack, 0.5f, layerMask) == false
				&& Physics.Raycast(downRayRightForward, 0.5f, layerMask) == false
				&& Physics.Raycast(downRayRightBack, 0.5f, layerMask) == false
				) {
					moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), -1.5f, Input.GetAxisRaw("Vertical"));
			
		}else 
		{
			moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		}
		
		// if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)){
		// 	moveInput = new Vector3(0, 0, Input.GetAxisRaw("Vertical"));
		// }
		// else if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)){
		// 	moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
		// }
		
		
		Vector3 moveVelocity = moveInput.normalized * moveSpeed;
		playerController.Move(moveVelocity);

		//ray
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
