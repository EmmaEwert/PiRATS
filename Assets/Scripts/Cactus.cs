using KITTY;
using UnityEngine;

public class Cactus : MonoBehaviour {
	public GameObject thornPrefab;
	[TiledProperty, HideInInspector] public float delay;
	[TiledProperty, HideInInspector] public float charge;

	float phase;

	public Animator animator => GetComponentInChildren<Animator>();

	void Start() {
		phase = delay;
	}

    void Update() {
		var Δt = Time.deltaTime;

		phase -= Δt;

		if (phase > 0) { // Wiggling
			animator.SetInteger("State", 0);
		} else if (phase > -charge) { // Charging
			animator.SetInteger("State", 1);
		} else { // Shoot and wiggle
			Shoot();
			phase = delay;
		}
    }

	void Shoot() {
		for (var angle = 0; angle < 360; angle += 45) {
			var thorn = Instantiate(thornPrefab, transform.position + Vector3.right, Quaternion.Euler(0f, 0f, angle));
		}
	}
}
