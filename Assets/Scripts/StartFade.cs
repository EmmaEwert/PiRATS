using UnityEngine;
using UnityEngine.SceneManagement;

public class StartFade : MonoBehaviour {
	float phase;

	CanvasGroup group => GetComponent<CanvasGroup>();

    void Update() {
        phase += Time.deltaTime;

		if (phase < 2f) {
			group.alpha = Mathf.Pow(phase / 2, 2f);
		} else if (phase > 4f) {
			group.alpha = Mathf.Pow((6f - phase) / 2f, 2f);
			if (phase > 6f) {
				PlayerPrefs.SetInt("Health", 0);
				SceneManager.LoadScene("Village", LoadSceneMode.Single);
			}
		} else {
			group.alpha = 1f;
		}
    }
}
