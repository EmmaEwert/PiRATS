using UnityEngine;
using UnityEngine.SceneManagement;

public class RatSparrow : MonoBehaviour {
	public Transform tail;
	public GameObject bombPrefab;

	float attack = 0f;
	float attackX = 0;
	float attackY = 0;

	float phase;
	float @throw;
	float hurt;

#pragma warning disable 0108
	Animator animator => GetComponentInChildren<Animator>();
#pragma warning restore 0108

	int health = 30;

	public void Hurt() {
		if (hurt > 0f) {
			return;
		}
		--health;
		if (health <= 0) {
			SceneManager.LoadScene("End", LoadSceneMode.Single);
		}
		hurt = 0.5f;
		animator.SetInteger("State", 5);
	}

    void Update() {
		var Δt = Time.deltaTime;

		hurt -= Δt;

		if (hurt > 0f) {
			return;
		}

		phase += Δt;

		if (phase < 21f) {
			if (phase < 20f) {
				ThrowPhase();
			} else {
				TurnPhase();
			}
		} else if (phase < 40f) {
			TailPhase();
		} else {
			phase = 0f;
			@throw = 0f;
		}
	}

	void ThrowPhase() {
		var t = Time.realtimeSinceStartup;
		var Δt = Time.deltaTime;
		for (var i = 0; i < tail.childCount; ++i) {
			var child = tail.GetChild(i);
			var tailPosition = new Vector3(Mathf.Cos(t + i / 2f) * Mathf.Pow(i, 1f / 1.5f) / (tail.childCount - 5f) * 2f, (Mathf.Sin(t + i / 3f) * 0.5f + 1.5f) * Mathf.Pow(i, 1f / 1.5f) / (tail.childCount - 11f) * 2f, -0.25f * -Mathf.Pow(i, 1f / 1.5f) * 2f);
			child.localPosition = tailPosition;
			
			var shadow = child.Find("Shadow");
			var shadowPosition = shadow.position;
			shadowPosition.z = 0f;
			shadow.position = shadowPosition;
		}
		if (phase < 5f) {
			animator.SetInteger("State", 0); // Idle
		} else if (phase < 10f) {
			animator.SetInteger("State", 1); // Throw
			@throw += Δt;
			if (@throw > 1f) {
				Throw();
				@throw -= 1f;
			}
		} else if (phase < 15f) {
			animator.SetInteger("State", 0);
		} else if (phase < 19.5) {
			animator.SetInteger("State", 1); // Throw
			@throw += Δt;
			if (@throw > 1f) {
				Throw();
				@throw = 0f;
			}
		} else {
			animator.SetInteger("State", 0);
		}
	}

	void Throw() {
		Instantiate(bombPrefab, transform.position + Vector3.down * 0.25f + Vector3.back * 1.75f, Quaternion.Euler(-60f, 0f, 0f));
	}

	void TurnPhase() {
		animator.SetInteger("State", 2); // Turn
		var t = Time.realtimeSinceStartup;
		var Δt = Time.deltaTime;
		for (var i = 0; i < tail.childCount; ++i) {
			var child = tail.GetChild(i);
			var tailPosition = new Vector3(Mathf.Cos(t + i / 2f) * Mathf.Pow(i, 1f / 1.5f) / (tail.childCount - 5f) * 2f, (Mathf.Sin(t + i / 3f) * 0.5f + 1.5f) * Mathf.Pow(i, 1f / 1.5f) / (tail.childCount - 11f) * 2f, -0.25f * -Mathf.Pow(i, 1f / 1.5f) * 2f);
			var tailPosition2 = new Vector3(Mathf.Cos(t + i / 2f) * Mathf.Pow(i, 1f / 1.5f) / (tail.childCount - 5f) + 0.5f, (Mathf.Sin(t + i / 3f) * 0.5f + 0.5f) * Mathf.Pow(i, 1f / 1.5f) / (tail.childCount - 11f) + 1f, -0.25f * Mathf.Pow(i, 1f / 1.5f));
			child.localPosition = Vector3.Lerp(tailPosition, tailPosition2, Mathf.Pow(phase - 10f, 0.125f));
			
			var shadow = child.Find("Shadow");
			var shadowPosition = shadow.position;
			shadowPosition.z = 0f;
			shadow.position = shadowPosition;
		}
	}

	void TailPhase() {
		animator.SetInteger("State", 3); // Lookback idle
		var t = Time.realtimeSinceStartup;
		var Δt = Time.deltaTime;
		this.attack += Δt / 3f;

		if (this.attack > 1.625f) {
			this.attack -= 1.125f;
		}

		var attack = Mathf.PingPong(this.attack, 1.125f);

		if (attack < 1.0f) {
			var player = GameObject.FindObjectOfType<PlayerController>().transform;
			attackX = Mathf.Lerp(attackX, (player.position.x - transform.position.x) * 1.25f, Δt);
			attackY = Mathf.Lerp(attackY, (player.position.y - transform.position.y) / tail.childCount * 1.25f, Δt);
		}

		for (var i = 0; i < tail.childCount; ++i) {
			var child = tail.GetChild(i);
			var tailPosition = new Vector3(Mathf.Cos(t + i / 2f) * Mathf.Pow(i, 1f / 1.5f) / (tail.childCount - 5f) + 0.5f, (Mathf.Sin(t + i / 3f) * 0.5f + 0.5f) * Mathf.Pow(i, 1f / 1.5f) / (tail.childCount - 11f) + 1f, -0.25f * Mathf.Pow(i, 1f / 1.5f));
			var attackPosition = Vector3.ClampMagnitude(new Vector3(attackX * Mathf.Pow((float)i / (tail.childCount - 4f), 0.5f) + 0.5f / (i + 1), -attackY * -1.0f * i / 2f + 1f / (i + 1), -attackY * -0.75f * i), i / 0.5f);
			var attackAlpha = Mathf.Pow(attack * Mathf.Pow(((tail.childCount - i) / (tail.childCount - 1f)), 1f / 32f), 16f);
			var player = FindObjectOfType<PlayerController>();
			var newPosition = Vector3.Lerp(tailPosition, attackPosition, attackAlpha);
			child.localPosition = newPosition;
			if (Vector3.Distance(child.position, player.transform.position + new Vector3(1f, 0f, 0f)) < 1.5f) {
				player.Hurt();
			} else {
				//Debug.Log($"{i}: {Vector3.Distance(child.position, player.transform.position)}");
			}
			
			var shadow = child.Find("Shadow");
			var shadowPosition = shadow.position;
			shadowPosition.z = 0f;
			shadow.position = shadowPosition;
		}
    }
}
