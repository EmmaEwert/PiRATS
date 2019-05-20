using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour {
	public GameObject smokePrefab;

	float age;

#pragma warning disable 0108
	Rigidbody2D rigidbody => GetComponent<Rigidbody2D>();
#pragma warning restore 0108

    void FixedUpdate() {
		rigidbody.MovePosition(transform.position + (transform.right * -7.5f + transform.up * 7.5f) * Time.deltaTime);
		var position = transform.Find("Sprite").position;
		position.z += age * Time.fixedDeltaTime ;
		if (position.z >= 0f) {
			position.z = 0f;
			Destroy(this);
			Destroy(transform.Find("Shadow").gameObject);
			Destroy(transform.Find("Collider").gameObject);
		}
		transform.Find("Sprite").position = position;
		age += Time.deltaTime;
    }

	void OnCollisionEnter2D(Collision2D collision) {
		Instantiate(smokePrefab, transform.position + transform.right * -0.5f + transform.up * 0.5f + transform.forward * -0.5f, Quaternion.Euler(-45f, 0f, 0f));
		Destroy(gameObject);
		var cactus = collision.transform.GetComponent<Cactus>();
		if (cactus) {
			cactus.Hurt();
		}
		var ratSparrow = collision.transform.GetComponent<RatSparrow>();
		if (ratSparrow) {
			ratSparrow.Hurt();
		}
	}
}
