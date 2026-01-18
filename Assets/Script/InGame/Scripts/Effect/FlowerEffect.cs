using UnityEngine;

public class FlowerEffect : MonoBehaviour {

    [SerializeField] private ParticleSystem ps;
    private ParticleSystem.MainModule main;
    private float speed = 0.5f;
    private float intensity = 3f;

    void Awake() {
        main = ps.main;
    }

    void Update() {
        float t = Mathf.Repeat(Time.time * speed, 1f);
        Color c = Color.HSVToRGB(t, 1f, 1f, true);
        c *= intensity;

        main.startColor = new ParticleSystem.MinMaxGradient(c);
    }
}
