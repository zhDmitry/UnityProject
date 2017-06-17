using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectable {
	protected override void OnRabitHit (HeroRabit rabit) {
		this.CollectedHide ();
		LevelController.current.addCoins (1);
	}
}
