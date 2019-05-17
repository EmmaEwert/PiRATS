using KITTY;
using TMPro;
using UnityEngine;

public class Sign : MonoBehaviour {
	public GameObject actionIndicator;
	public GameObject canvas;
	public TextMeshProUGUI textMesh;
	[TiledProperty, HideInInspector] public string text;

	PlayerController player;

	void Start() {
		textMesh.text = text;
	}

	void Update() {
		if (player && Input.GetKeyDown(KeyCode.E)) {
			canvas.SetActive(actionIndicator.activeSelf);
			player.enabled = !actionIndicator.activeSelf;
			actionIndicator.SetActive(!actionIndicator.activeSelf);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		player = other.GetComponentInParent<PlayerController>();
		if (player) {
			actionIndicator.SetActive(true);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (player) {
			actionIndicator.SetActive(false);
			player = null;
		}
	}
}
