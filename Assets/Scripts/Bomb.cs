using UnityEngine;

public class Bomb : MonoBehaviour {
	public GameObject explosionPrefab;

	Vector3 source;
	Vector3 target;

	float phase;

    void Start() {
		source = transform.position;
        target = FindObjectOfType<PlayerController>().transform.position;
    }

    void Update() {
		var Δt = Time.deltaTime;
		phase += Δt;

		var α = Mathf.Clamp(phase, 0f, 2f) / 2f;
		var position = Vector3.Lerp(source, target, α);
		position.z += Mathf.Pow((α - 0.5f) * 4f, 2f) - Mathf.Pow(4f / 2f, 2f);
		transform.position = position;

		if (phase > 4f) {
			Instantiate(explosionPrefab, transform.position + Vector3.left + Vector3.forward * 0.5f, Quaternion.Euler(-60f, 0f, 0f));
			Destroy(gameObject);
		}
    }
}
