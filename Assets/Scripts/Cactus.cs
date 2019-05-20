﻿using KITTY;
using UnityEngine;

public class Cactus : MonoBehaviour {
	public GameObject thornPrefab;
	public GameObject silverCoinPrefab;
	public GameObject goldCoinPrefab;
	[TiledProperty, HideInInspector, SerializeField] public float delay;
	[TiledProperty, HideInInspector, SerializeField] public float charge;

	float phase;
	bool cardinal;
	bool inRange;

	float hurt;
	int health = 3;

	public Animator animator => GetComponentInChildren<Animator>();

	public void Hurt() {
		hurt = 0.5f;
		--health;
		transform.Find("enemy_hit").GetComponent<AudioSource>().Play();
		if (health == 0) {
			// Spawn coins
			var silver = Random.Range(0, 6);
			var gold = Random.Range(0, 3);
			for (var i = 0; i < silver; ++i) {
				var position = Random.insideUnitCircle;
				//Instantiate(silverCoinPrefab, transform.position + new Vector3(position.x, position.y), Quaternion.Euler(-45f, 0f, 0f));
			}
			for (var i = 0; i < gold; ++i) {
				var position = Random.insideUnitCircle;
				//Instantiate(goldCoinPrefab, transform.position + new Vector3(position.x, position.y), Quaternion.Euler(-45f, 0f, 0f));
			}
			Destroy(gameObject);
		}
	}

	void Start() {
		phase = delay;
	}

    void Update() {
		var Δt = Time.deltaTime;

		if (hurt > 0f) {
			animator.SetInteger("State", 2);
			hurt -= Δt;
			return;
		}

		phase -= Δt;

		if (phase > 0) { // Wiggling
			animator.SetInteger("State", 0);
			if (inRange) {
				transform.Translate(-(transform.position - FindObjectOfType<PlayerController>().transform.position).normalized * Time.deltaTime);
				var position = transform.position;
				position.z = 0f;
				transform.position = position;
			}
		} else if (phase > -charge && inRange) { // Charging
			animator.SetInteger("State", 1);
		} else if (inRange) { // Shoot and wiggle
			Shoot();
			phase = delay;
		} else {
			animator.SetInteger("State", 0);
			phase = 0f;
		}
    }

	void Shoot() {
		for (var angle = cardinal ? 45f : 0; angle < 360; angle += 90) {
			var thorn = Instantiate(thornPrefab, transform.position + Vector3.right, Quaternion.Euler(0f, 0f, angle));
		}
		cardinal = !cardinal;
		transform.Find("cactus_needleshot").GetComponent<AudioSource>().Play();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponentInParent<PlayerController>()) {
			inRange = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.GetComponentInParent<PlayerController>()) {
			//inRange = false;
		}
	}
}
