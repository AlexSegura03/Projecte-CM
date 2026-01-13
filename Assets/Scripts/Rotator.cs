using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Header("Rotació")]
    public float rotationSpeed = 60f; // graus per segon
    public float tiltX = 10f;          // inclinació fixa
    public float tiltZ = 5f;

    [Header("Bot")]
    public float bounceHeight = 0.5f;  // alçada del bot
    public float bounceSpeed = 2f;     // velocitat del bot

    private Vector3 startPos;
    private float yRotation;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // BOT (moviment vertical)
        float bounce = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = startPos + Vector3.up * bounce;

        // ROTACIÓ només en Y
        yRotation += rotationSpeed * Time.deltaTime;

        // Apliquem rotació + inclinació fixa
        transform.rotation = Quaternion.Euler(
            tiltX,
            yRotation,
            tiltZ
        );
    }
}
