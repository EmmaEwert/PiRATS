using UnityEngine;

public class Needle : MonoBehaviour {
	public GameObject smokePrefab;

	void OnCollisionEnter2D(Collision2D collision) {
		Instantiate(smokePrefab, transform.position + transform.right * -1f + transform.up + transform.forward * -0.5f, Quaternion.Euler(-45f, 0f, 0f));
		Destroy(gameObject);
	}
}
