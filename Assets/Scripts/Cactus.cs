using KITTY;
using UnityEngine;

public class Cactus : MonoBehaviour {
	public GameObject thornPrefab;
	[TiledProperty, HideInInspector] public float delay;
	[TiledProperty, HideInInspector] public float charge;

	float phase;
	bool cardinal;
	bool inRange;

	public Animator animator => GetComponentInChildren<Animator>();

	void Start() {
		phase = delay;
	}

    void Update() {
		var Δt = Time.deltaTime;

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
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponentInParent<PlayerController>()) {
			inRange = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.GetComponentInParent<PlayerController>()) {
			inRange = false;
		}
	}
}
