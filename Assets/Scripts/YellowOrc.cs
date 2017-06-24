using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowOrc : MonoBehaviour {

	public float speed = 1;
	bool isGrounded = false;
	bool JumpActive = false;
	float JumpTime = 0f;
	public float MaxJumpTime = 2f;
	public float JumpSpeed = 2f;

	public Vector3 MoveBy;

	Rigidbody2D myBody = null;
	SpriteRenderer sr = null;
	Animator animator = null;

	// Use this for initialization
	void Start () {
		myBody = this.GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator> ();

		pointA = this.transform.position;
		pointB = this.pointA + MoveBy;

	}

	// Update is called once per frame
	void Update () {
		if (!isDying) {
			UpdateMove ();
			UpdateGrounded ();
			UpdateJump ();
		} else {
			UpdateDie ();
		}
	}

	void UpdateMove() {
		float value = this.getDirection ();

		if (Mathf.Abs (value) > 0) {
			Vector2 vel = myBody.velocity;
			vel.x = value * speed;
			myBody.velocity = vel;
			animator.SetBool ("run", true);
		} else {
			animator.SetBool ("run", false);
		}

		if(value < 0) {
			sr.flipX = false;
		} else if(value > 0) {
			sr.flipX = true;
		}
	}

	public enum Mode {
		GoToA,
		GoToB,
		Attack
	}

	public GameObject prefabCarrot;
	float last_carrot = 0;
	public float carrotIntervalSeconds = 2.0f;
	void launchCarrot(Vector2 direction) {
		if (Time.time - last_carrot > carrotIntervalSeconds) {
			last_carrot = Time.time;
			//Створюємо копію Prefab
			GameObject obj = GameObject.Instantiate (this.prefabCarrot);
			//Розміщуємо в просторі
			obj.transform.position = this.transform.position;
			//Запускаємо в рух
			Carrot carrot = obj.GetComponent<Carrot> ();
			carrot.launch (direction);
			this.animator.SetTrigger ("throw");
		}
	}

	private Vector3 pointA, pointB;
	private Mode mode = Mode.GoToA;
	public float carrotLaunchRadius = 5.0f;
	float getDirection() {
		Vector3 rabit_pos = HeroRabit.lastRabbit.transform.position;
		Vector3 my_pos = this.transform.position;

		if(Mathf.Abs(rabit_pos.x - my_pos.x) < carrotLaunchRadius) {
			this.launchCarrot (rabit_pos - my_pos);
		}



		switch (mode) {
		case Mode.GoToA:
			if (hasArrived (my_pos, pointA)) {
				mode = Mode.GoToB;
			}
			break;
		case Mode.GoToB:
			if (hasArrived (my_pos, pointB)) {
				mode = Mode.GoToA;
			}
			break;
		case Mode.Attack:
			if(my_pos.x < rabit_pos.x) {
				return 1;
			} else {
				return -1;
			}
		}

		switch (mode) {
		case Mode.GoToA:
			if (my_pos.x < pointA.x) {
				return 1;
			} else {
				return -1;
			}
		case Mode.GoToB:
			if (my_pos.x < pointB.x) {
				return 1;
			} else {
				return -1;
			}
		default:
			return 0;
		}
	}

	bool hasArrived(Vector3 pos, Vector3 target) {
		pos.z = 0;
		target.z = 0;
		//		pos.y = 0;
		//		target.y = 0;
		return Vector3.Distance(pos, target) < 0.5f;
	}

	void UpdateGrounded() {
		//class HeroRabit, void FixedUpdate()
		Vector3 from = transform.position + Vector3.up * 0.3f;
		Vector3 to = transform.position + Vector3.down * 0.1f;
		int layer_id = 1 << LayerMask.NameToLayer ("Ground");
		//Перевіряємо чи проходить лінія через Collider з шаром Ground
		RaycastHit2D hit = Physics2D.Linecast(from, to, layer_id);
		if(hit) {
			isGrounded = true;
		} else {
			isGrounded = false;
		}
		//Намалювати лінію (для розробника)
		Debug.DrawLine (from, to, Color.red);
	}

	void UpdateJump() {
		this.JumpActive = false;

		if(this.JumpActive) {
			//Якщо кнопку ще тримають
			if(Input.GetButton("Jump")) {
				this.JumpTime += Time.deltaTime;
				if (this.JumpTime < this.MaxJumpTime) {
					Vector2 vel = myBody.velocity;
					vel.y = JumpSpeed * (1.0f - JumpTime / MaxJumpTime);
					myBody.velocity = vel;
				}
			} else {
				this.JumpActive = false;
				this.JumpTime = 0;
			}
		}

		if(this.isGrounded) {
			animator.SetBool ("jump", false);
		} else {
			animator.SetBool ("jump", true);
		}
	}

	private void UpdateDie () {

		if (isDying) {
			timeLeftToDie -= Time.deltaTime;
			if (timeLeftToDie <= 0) {
				isDying = false;
				Destroy (this.gameObject);
				//LevelController.current.onRabitDeath (this.gameObject);
			}
		}
	}

	public float dieAnimationTime = 1;
	float timeLeftToDie;
	bool isDying = false;


	public void Die() {
		if (isDying) {
			return;
		}

		if (this.isGrounded) {
			isDying = true;
			//animator.SetBool ("die", true);
			animator.SetTrigger("die");
			timeLeftToDie = dieAnimationTime;
		} else {
			//LevelController.current.onRabitDeath (this.gameObject);
		}
	}


	void OnTriggerEnter2D(Collider2D collider) {
		HeroRabit heroController = collider.GetComponentInParent<HeroRabit> ();
		if(heroController != null) {
			GameObject rabbit = heroController.gameObject;
			if (rabbit.transform.position.y > this.transform.position.y + 1) {
				this.Die ();
				heroController.myBody.AddForce (new Vector2 (0, 15));
			} else {
				this.animator.SetTrigger ("attack");
				heroController.Die ();
			}
		}
	}
}
