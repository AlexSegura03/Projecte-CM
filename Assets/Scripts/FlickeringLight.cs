using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickeringLight : MonoBehaviour
{
    private Light lightToFlicker;

    [SerializeField, Range(0f, 3f)] private float minIntensity = 0.5f;
    [SerializeField, Range(0f, 3f)] private float maxIntensity = 1.2f;
    [SerializeField, Min(0f)] private float timeBetween = 0.1f;

    private float currentTimer;

    private void Awake()
    {
        lightToFlicker = GetComponent<Light>();
        ValidateIntensityBounds();
    }

    private void Update()
    {
        currentTimer += Time.deltaTime;

        if (currentTimer < timeBetween) return;

        lightToFlicker.intensity = Random.Range(minIntensity, maxIntensity);
        currentTimer = 0f;
    }

    private void ValidateIntensityBounds()
    {
        if (minIntensity <= maxIntensity) return;

        Debug.LogWarning("minIntensity és més gran que maxIntensity. S'intercanvien.");
        (minIntensity, maxIntensity) = (maxIntensity, minIntensity);
    }
}
