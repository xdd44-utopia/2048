using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControl : MonoBehaviour
{
    
	private float movementSpeed = 0.2f;
	private int status; //0: appear 1: still 2: moving 3: disappear
	private int value;
	private Vector3 currentPos;
	private Vector3 target;
	private float timer;

	private float formula(float t){
		return Mathf.Sqrt(t);
	}

	public void Start(){
		transform.localScale = new Vector3(0f, 0f, 0f);
		currentPos = transform.position;
		target = transform.position;
		status = 0;
		timer = 0;
	}
	public void Update(){
		switch (status){
			case 0:
				timer += Time.deltaTime;
				if (timer > movementSpeed) {
					timer = movementSpeed;
					status = 1;
				}
				transform.localScale = new Vector3(formula(timer / movementSpeed), formula(timer / movementSpeed), formula(timer / movementSpeed));
				if (timer == movementSpeed) {
					timer = 0;
				}
				break;
			case 1:
				transform.localScale = new Vector3(1, 1, 1);
				break;
			case 2:
				timer += Time.deltaTime;
				if (timer > movementSpeed) {
					timer = movementSpeed;
					status = 1;
				}
				transform.position = currentPos + (target - currentPos) * formula(timer / movementSpeed);
				if (timer == movementSpeed) {
					timer = 0;
					currentPos = transform.position;
				}
				break;
		}
	}

	public void move(Vector3 pos) {
		target = pos;
		status = 2;
	}

}
