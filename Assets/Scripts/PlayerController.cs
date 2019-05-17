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

		// Movement input is rotated around the X-axis to limit the speed on the depth axis.
        var movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		movement = Vector3.ClampMagnitude(movement, 1f);
		movement = Quaternion.Euler(transform.localRotation.eulerAngles.x, 0f, 0f) * movement;
		rigidbody.MovePosition(rigidbody.position + new Vector2(movement.x, movement.y) * Δt * Speed);

		// Angle of attack is just based on angle from mouse position to a centered vertical line.
		var mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		var aim = Vector3.SignedAngle(Vector3.up, mouse - new Vector3(0.5f, 0.5f), Vector3.forward);
		aimIndicator.localRotation = Quaternion.Euler(-transform.localRotation.eulerAngles.x, 0f, aim);

		// Arrows fire in the direction the indicator is showing. TODO: Fix indicator angle.
		if (Input.GetButtonDown("Fire1")) {
			Instantiate(arrowPrefab, transform.position + Vector3.right * 0.5f, Quaternion.Euler(0f, 0f, aim - 45f));
		}
    }
}
