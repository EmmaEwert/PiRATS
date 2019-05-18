using UnityEngine;

public class Needle : MonoBehaviour {
	public GameObject smokePrefab;

	void Update() {
		var sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite;
		transform.Find("Shadow").GetComponent<SpriteRenderer>().sprite = sprite;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		Instantiate(smokePrefab, transform.position + transform.right * -0.5f + transform.up * 0.5f + transform.forward * -0.5f, Quaternion.Euler(-45f, 0f, 0f));
		Destroy(gameObject);
	}
}
