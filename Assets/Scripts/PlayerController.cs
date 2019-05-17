using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
	const float Speed = 5f;

#pragma warning disable 0108
	Rigidbody2D	rigidbody => GetComponent<Rigidbody2D>();
#pragma warning restore 0108

    void Update() {
		var Δt = Time.deltaTime;
        var movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		movement = Vector3.ClampMagnitude(movement, 1f);
		movement = Quaternion.Euler(30f, 0f, 0f) * movement;
		rigidbody.MovePosition(rigidbody.position + new Vector2(movement.x, movement.y) * Δt * Speed);
    }
}
