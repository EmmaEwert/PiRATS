using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
	const float Speed = 5f;

	public Transform aimIndicator;
	public GameObject arrowPrefab;

#pragma warning disable 0108
	Rigidbody2D	rigidbody => GetComponent<Rigidbody2D>();
#pragma warning restore 0108

    void Update() {
		var Δt = Time.deltaTime;

        var movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		movement = Vector3.ClampMagnitude(movement, 1f);
		movement = Quaternion.Euler(30f, 0f, 0f) * movement;
		rigidbody.MovePosition(rigidbody.position + new Vector2(movement.x, movement.y) * Δt * Speed);

		var mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		var aim = Vector2.SignedAngle(Vector2.up, mouse - Vector3.one * 0.5f);
		aimIndicator.localRotation = Quaternion.Euler(60f, 0f, aim);

		if (Input.GetButtonDown("Fire1")) {
			Instantiate(arrowPrefab, transform.position + Vector3.right * 0.5f, Quaternion.Euler(0f, 0f, aim - 45f));
		}
    }
}
