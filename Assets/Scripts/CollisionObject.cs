using KITTY;
using UnityEngine;

public class CollisionObject : MonoBehaviour {
	[TiledProperty] public float width = 2;
	[TiledProperty] public float height = 1;

    void Start() {
        transform.localScale = new Vector3(width, height, 1);
    }
}
