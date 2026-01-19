using UnityEngine;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(AudioSource))]
public class OldIncandescentLight : MonoBehaviour
{
    private Light bulb;
    private AudioSource audioSource;

    [Header("Intensitat")]
    public float baseIntensity = 1.2f;
    public float intensityVariation = 0.25f;
    public float flickerSpeed = 0.5f;

    [Header("Color (incandescent)")]
    public Color warmColor = new Color(1f, 0.55f, 0.25f);
    public Color hotterColor = new Color(1f, 0.65f, 0.35f);

    [Header("So")]
    public float baseVolume = 0.15f;
    public float volumeVariation = 0.1f;
    public float pitchVariation = 0.05f;

    private float noiseOffset;

    void Awake()
    {
        bulb = GetComponent<Light>();
        audioSource = GetComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.spatialBlend = 1f; // 3D real
        audioSource.Play();

        noiseOffset = Random.Range(0f, 1000f);
    }

    void Update()
    {
        float t = Time.time * flickerSpeed;

        // Intensitat (Perlin Noise)
        float noise = Mathf.PerlinNoise(noiseOffset, t);
        float intensity = baseIntensity + (noise - 0.5f) * intensityVariation * 2f;
        intensity = Mathf.Max(0f, intensity);
        bulb.intensity = intensity;

        // Color segons intensitat (filament)
        float colorT = Mathf.InverseLerp(
            baseIntensity - intensityVariation,
            baseIntensity + intensityVariation,
            intensity
        );
        Color targetColor = Color.Lerp(warmColor, hotterColor, colorT);
        bulb.color = Color.Lerp(bulb.color, targetColor, Time.deltaTime * 5f);


        // SO: volum i pitch segueixen la intensitat
        audioSource.volume = baseVolume + colorT * volumeVariation;
        audioSource.pitch = 1f + (noise - 0.5f) * pitchVariation;
    }
}
