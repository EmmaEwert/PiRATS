using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	const float Speed = 5f;

	public Transform aimIndicator;
	public GameObject gameOverScreen;
	public Transform healthIndicator;
	public GameObject arrowPrefab;
	public GameObject heartEmptyPrefab;
	public GameObject heartHalfPrefab;
	public GameObject heartFullPrefab;

	float hurt;
	int health = 0;
	int maxHealth = 4;
	float arrowDelay;

	Animator animator => transform.Find("Sprite").GetComponent<Animator>();
#pragma warning disable 0108
	SpriteRenderer renderer => transform.Find("Sprite").GetComponent<SpriteRenderer>();
	Rigidbody2D	rigidbody => GetComponent<Rigidbody2D>();
#pragma warning restore 0108

	public void Hurt() {
		hurt = 0.5f;
		health -= 1;
		UpdateHealthUI();
		if (health <= 0) {
			gameOverScreen.SetActive(true);
			animator.SetInteger("State", 5);
			enabled = false;
			Destroy(rigidbody);
		}
		PlayerPrefs.SetInt("Health", health);
	}

	public void Respawn() {
		SceneManager.LoadScene("Village", LoadSceneMode.Single);
	}

	void Start() {
		health = PlayerPrefs.GetInt("Health");
		if (SceneManager.GetActiveScene().name == "Village" && health > 0) {
			transform.position = new Vector3(34, 20);
		} else if (health <= 0) {
			PlayerPrefs.SetInt("Health", maxHealth);
			health = maxHealth;
		}
		UpdateHealthUI();
	}

    void FixedUpdate() {
		var Δt = Time.fixedDeltaTime;

		arrowDelay -= Δt;

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
		if (Input.GetButton("Fire1") && arrowDelay <= 0f) {
			Instantiate(arrowPrefab, transform.position + Vector3.right, Quaternion.Euler(0f, 0f, aim - 45f));
			arrowDelay = 0.75f;
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

	void OnTriggerEnter2D(Collider2D other) {
		var warp = other.GetComponent<Warp>();
		if (warp) {
			SceneManager.LoadScene(warp.destination, LoadSceneMode.Single);
		}
	}
}
