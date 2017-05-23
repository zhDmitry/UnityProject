using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRabit : MonoBehaviour {
	public float speed = 1;
	Rigidbody2D myBody = null;
	public bool isGrounded = false;
	public bool JumpActive = false;
	float JumpTime = 0f;
	public float MaxJumpTime = 2f; public float JumpSpeed = 2f;

	// Use this for initialization
	void Start () {
		LevelController.current.setStartPosition (transform.position);
		myBody = this.GetComponent<Rigidbody2D> ();
	}
		
	// Update is called once per frame
	void Update () {
		float value = Input.GetAxis ("Horizontal");
		Animator animator = GetComponent<Animator> (); 
		if(this.isGrounded) {
			animator.SetBool ("jump", false);
		} else {
			animator.SetBool ("jump", true);
		}
		if(Mathf.Abs(value) > 0) {
			animator.SetBool ("run", true); } 
		else {
			animator.SetBool ("run", false); 
		}

	}
	void FixedUpdate () { //[-1, 1]
		

	
		Vector3 from = transform.position + Vector3.up * 0.3f; Vector3 to = transform.position + Vector3.down * 0.1f; int layer_id = 1 << LayerMask.NameToLayer ("Ground");
		//Перевіряємо чи проходить лінія через Collider з шаром Ground
		RaycastHit2D hit = Physics2D.Linecast(from, to, layer_id);
		if(hit) {
			isGrounded = true;
		} else {
			isGrounded = false;
		}
		//Намалювати лінію (для розробника)
		Debug.DrawLine (from, to, Color.red);
		float value = Input.GetAxis ("Horizontal");


		if (Mathf.Abs (value) > 0) {
			Vector2 vel = myBody.velocity;
			vel.x = value * speed;
			myBody.velocity = vel;
		} 
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		if(value < 0) {
			sr.flipX = true;
		} else if(value > 0) {
			sr.flipX = false;
		}

		if(Input.GetButtonDown("Jump") && isGrounded) {
			this.JumpActive = true; 
		}
		if(this.JumpActive && this.JumpTime < this.MaxJumpTime) {
			//Якщо кнопку ще тримають if(Input.GetButton("Jump")) {
			this.JumpTime += Time.deltaTime;
			Vector2 vel = myBody.velocity;
			vel.y = JumpSpeed * (1.0f - JumpTime / MaxJumpTime);
			myBody.velocity = vel;
		} else {
			this.JumpActive = false;
			this.JumpTime = 0; 
		}

	}
}
