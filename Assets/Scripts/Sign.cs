﻿using KITTY;
using TMPro;
using UnityEngine;

public class Sign : MonoBehaviour {
	public GameObject actionIndicator;
	public GameObject canvas;
	public TextMeshProUGUI textMesh;
	[TiledProperty, HideInInspector, SerializeField] public string text;

	PlayerController player;

	void Start() {
		textMesh.text = text;
	}

	void Update() {
		// Flip active state of textbox, player, and indicator if the player presses E within range.
		if (player && Input.GetKeyDown(KeyCode.E)) {
			canvas.SetActive(actionIndicator.activeSelf);
			player.enabled = !actionIndicator.activeSelf;
			actionIndicator.SetActive(!actionIndicator.activeSelf);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		// Show indicator when player is in range.
		var player = other.GetComponentInParent<PlayerController>();
		if (player) {
			this.player = player;
			actionIndicator.SetActive(true);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		// Hide indicator when player goes out of range.
		if (player && other.GetComponentInParent<PlayerController>()) {
			actionIndicator.SetActive(false);
			player = null;
		}
	}
}
