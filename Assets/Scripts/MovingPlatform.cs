using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
	
	public Vector3 MoveBy;
	public float WaitTime = 0.5f;
	public float Speed = 2f;
	Vector3 pointA;
	Vector3 pointB;

	float to_wait = 0;
	bool is_moving_to_A = false;

	// Use this for initialization
	void Start () {
		this.pointA = this.transform.position;
		this.pointB = this.pointA + MoveBy;	
	}

	bool isArrived(Vector3 pos, Vector3 target) {
		pos.z = 0;
		target.z = 0;
		return Vector3.Distance(pos, target) < 0.02f;
	}

	// Update is called once per frame
	void Update () {

		to_wait -= Time.deltaTime;
		if (to_wait > 0)
			return;
		
		Vector3 my_pos = this.transform.position;
		Vector3 target;
		if(is_moving_to_A) {
			target = this.pointA;
		} else {
			target = this.pointB;
		}



		if (isArrived (target, my_pos)) {
			is_moving_to_A = !is_moving_to_A;
			to_wait = this.WaitTime;
		} else {
			Vector3 destination = target - my_pos;
			destination.z = 0;
			float move = this.Speed = Time.deltaTime;
			float distance = Vector3.Distance (destination, my_pos);

			Vector3 move_vector = destination.normalized * Mathf.Min (move, distance);
			this.transform.position += move_vector;
		}
	}


}