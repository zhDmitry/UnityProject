using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour {

	public float slowdown = 0.5f;
	Vector3 lastPosition;
	void Awake() {
		lastPosition = Camera.main.transform.position;
	}
	void LateUpdate() {
		Vector3 new_position = Camera.main.transform.position; Vector3 diff = new_position - lastPosition; lastPosition = new_position;
		Vector3 my_pos = this.transform.position; //Рухаємо фон в туж сторону що й камера але з іншою //швидкістю
		my_pos += slowdown * diff;
		this.transform.position = my_pos;
	}
}
