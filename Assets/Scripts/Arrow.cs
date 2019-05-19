using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour {
	public GameObject smokePrefab;

#pragma warning disable 0108
	Rigidbody2D rigidbody => GetComponent<Rigidbody2D>();
#pragma warning restore 0108

    void Update() {
		rigidbody.MovePosition(transform.position + (transform.right * -5f + transform.up * 5f) * Time.deltaTime);
    }

	void OnCollisionEnter2D(Collision2D collision) {
		Instantiate(smokePrefab, transform.position + transform.right * -0.5f + transform.up * 0.5f + transform.forward * -0.5f, Quaternion.Euler(-45f, 0f, 0f));
		Destroy(gameObject);
		var cactus = collision.transform.GetComponent<Cactus>();
		Debug.Log(collision.transform);
		if (cactus) {
			cactus.Hurt();
		}
	}
}
