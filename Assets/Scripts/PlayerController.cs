﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
	const float Speed = 5f;

	public Transform aimIndicator;
	public Transform healthIndicator;
	public GameObject arrowPrefab;
	public GameObject heartEmptyPrefab;
	public GameObject heartHalfPrefab;
	public GameObject heartFullPrefab;

	float hurt;
	int health = 6;
	int maxHealth = 6;

	Animator animator => transform.Find("Sprite").GetComponent<Animator>();
#pragma warning disable 0108
	SpriteRenderer renderer => transform.Find("Sprite").GetComponent<SpriteRenderer>();
	Rigidbody2D	rigidbody => GetComponent<Rigidbody2D>();
#pragma warning restore 0108

	public void Hurt() {
		hurt = 0.5f;
		health -= 1;
		UpdateHealthUI();
	}

	void Start() {
		UpdateHealthUI();
	}

    void Update() {
		var Δt = Time.deltaTime;

		if (hurt > 0f) {
			animator.SetInteger("State", 4);
			hurt -= Δt;
			return;
		}

		// Movement input is rotated around the X-axis to limit the speed on the depth axis.
        var movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		movement = Vector3.ClampMagnitude(movement, 1f);
		movement = Quaternion.Euler(transform.localRotation.eulerAngles.x, 0f, 0f) * movement;
		rigidbody.MovePosition(rigidbody.position + new Vector2(movement.x, movement.y) * Δt * Speed);

		if (movement.magnitude < 0.125f) {
			animator.SetInteger("State", 0);
		} else {
			animator.SetFloat("Speed", movement.magnitude);
			if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y * 2f)) {
				renderer.flipX = movement.x < 0;
				renderer.transform.localPosition = movement.x < 0 ? Vector3.right * 2f: Vector3.zero;
				animator.SetInteger("State", 3);
			} else if (movement.y > 0) {
				animator.SetInteger("State", 2);
			} else {
				animator.SetInteger("State", 1);
			}
		}

		// Angle of attack is just based on angle from mouse position to a centered vertical line.
		var mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		var aim = Vector3.SignedAngle(Vector3.up, mouse - new Vector3(0.5f, 0.5f), Vector3.forward);
		aimIndicator.localRotation = Quaternion.Euler(-transform.localRotation.eulerAngles.x, 0f, aim + 45f);

		// Arrows fire in the direction the indicator is showing. TODO: Fix indicator angle.
		if (Input.GetButtonDown("Fire1")) {
			Instantiate(arrowPrefab, transform.position + Vector3.right, Quaternion.Euler(0f, 0f, aim - 45f));
		}
    }

	void UpdateHealthUI() {
		foreach (Transform child in healthIndicator) {
			Destroy(child.gameObject);
		}
		for (var i = 0; i < maxHealth; i += 2) {
			if (health >= i + 2) {
				Instantiate(heartFullPrefab, Vector3.zero, Quaternion.identity, healthIndicator);
			} else if (health >= i + 1) {
				Instantiate(heartHalfPrefab, Vector3.zero, Quaternion.identity, healthIndicator);
			} else {
				Instantiate(heartEmptyPrefab, Vector3.zero, Quaternion.identity, healthIndicator);
			}
		}
	}
}
