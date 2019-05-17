using UnityEngine;

public class RatSparrow : MonoBehaviour {
	public Transform tail;

	float attack = 0f;
	float attackX = 0;

    void Update() {
		var t = Time.realtimeSinceStartup;
		var Δt = Time.deltaTime;

		this.attack += Δt / 6f;

		if (this.attack > 1.625f) {
			this.attack -= 1.125f;
		}

		var attack = Mathf.PingPong(this.attack, 1.125f);

		if (attack < 0.875f) {
			var player = GameObject.FindObjectOfType<PlayerController>().transform;
			attackX = player.position.x - transform.position.x;
		}


		for (var i = 0; i < tail.childCount; ++i) {
			var child = tail.GetChild(i);
			var tailPosition = new Vector3(Mathf.Cos(t + i) * i / 7f, (Mathf.Sin(t + i) * 0.5f + 0.5f) * i / 5f, -0.5f * i);
			var attackPosition = Vector3.ClampMagnitude(new Vector3(attackX * 1.5f, -0.5f * i, -0.5f * i), i / 1.5f);
			child.localPosition = Vector3.Lerp(tailPosition, attackPosition, Mathf.Pow(attack * Mathf.Pow(((8 - i) / 7f), 1f / 32f), 16f));
			var shadow = child.Find("Shadow");
			var shadowPosition = shadow.position;
			shadowPosition.z = 0f;
			shadow.position = shadowPosition;
		}
    }
}
