using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour {

	protected abstract void OnRabitHit (HeroRabit rabit);

	public bool hideAnimation = false;

	void OnTriggerEnter2D(Collider2D collider) {
			HeroRabit heroController = collider.GetComponentInParent<HeroRabit> ();
			if(heroController != null) {
				this.OnRabitHit (heroController);
			}
	}

	public void CollectedHide() {
		Destroy(this.gameObject);
	}
}
