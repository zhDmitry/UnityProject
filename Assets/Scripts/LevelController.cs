using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

	public static LevelController current;
	Vector3 startingPosition;

	// Use this for initialization
	void Awake () {
		current = this;
	}
	public void onRabitDeath(HeroRabit rabit) {
		//При смерті кролика повертаємо на початкову позицію
		rabit.transform.position = this.startingPosition;
		coins = 0;
	}
	public void setStartPosition(Vector3 pos) {
		this.startingPosition = pos;
	}
	

	private int coins = 0;
	public void addCoins(int n) {
		coins += n;
	}
}
