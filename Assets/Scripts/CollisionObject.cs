using KITTY;
using UnityEngine;

public class CollisionObject : MonoBehaviour {
	[TiledProperty, SerializeField] public float width = 2;
	[TiledProperty, SerializeField] public float height = 1;

    void Start() {
        transform.localScale = new Vector3(width, height, 1);
    }
}
