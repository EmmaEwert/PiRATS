using UnityEngine;

public class Explosion : MonoBehaviour {
	float time;
	
	void Start() {
		transform.Find("explosion_effect").GetComponent<AudioSource>().Play();
	}
	
    void Update() {
        time += Time.deltaTime;
		if (time > 0.75f) {
			Destroy(gameObject);
		}

		var player = GameObject.FindObjectOfType<PlayerController>();
		if (Vector3.Distance(transform.position + Vector3.right + Vector3.back * 0.5f + Vector3.up * 0.5f, player.transform.position) < 3f) {
			player.Hurt();
		}
    }
}
