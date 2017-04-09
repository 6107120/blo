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
		Debug.DrawRay(ray.origin, ray.direction, Color.red);
		Ray upBaseray = new Ray(transform.position + new Vector3(0, -0.4f, 0), transform.forward);
		RaycastHit upBasehit;
		//Debug.DrawRay(upBaseray.origin, upBaseray.direction, Color.green);
		Ray upRay = new Ray(transform.position, transform.up);
		RaycastHit upHit;
		//Debug.DrawRay(upRay.origin, upRay.direction, Color.cyan);
		Ray upForwardRay = new Ray(transform.position + Vector3.up, transform.forward);
		RaycastHit upForwardHit;
		//Debug.DrawRay(upForwardRay.origin, upForwardRay.direction, Color.yellow);
		Ray downBaseray = new Ray(transform.position + new Vector3(0, 0.4f, 0), transform.forward);
		RaycastHit downBasehit;
		Ray downRay = new Ray(transform.position, transform.up);
		RaycastHit downHit;
		//Debug.DrawRay(upRay.origin, upRay.direction, Color.cyan);
		Ray downForwardRay = new Ray(transform.position + Vector3.up, transform.forward);
		RaycastHit downForwardHit;

		if(Physics.Raycast(upBaseray,out upBasehit, 0.5f, layerMask)) {
			Debug.DrawRay(upBaseray.origin, upBaseray.direction, Color.green);
			if(Physics.Raycast(upRay,out upHit, 1f, layerMask) == false) {
				Debug.DrawRay(upRay.origin, upRay.direction, Color.cyan);
				if(Physics.Raycast(upForwardRay,out upForwardHit, 1f, layerMask) == false) {
					Debug.DrawRay(upForwardRay.origin, upForwardRay.direction, Color.yellow);
					moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 1.5f, Input.GetAxisRaw("Vertical"));
				}
			}
		}else
		if(Physics.Raycast(downBaseray,out downBasehit, 0.5f, layerMask) == false) {
			Debug.DrawRay(downBaseray.origin, downBaseray.direction, Color.green);
			if(Physics.Raycast(downRay,out downHit, 1f, layerMask) == false) {
				Debug.DrawRay(downRay.origin, downRay.direction, Color.cyan);
				if(Physics.Raycast(downForwardRay,out downForwardHit, 1f, layerMask) == false) {
					Debug.DrawRay(downForwardRay.origin, downForwardRay.direction, Color.yellow);
					moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), -1.5f, Input.GetAxisRaw("Vertical"));
				}
			}
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
