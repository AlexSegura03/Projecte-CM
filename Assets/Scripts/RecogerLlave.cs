using UnityEngine;

public class RecogerLlave : MonoBehaviour
{
    // Esta variable es estática: se puede leer desde cualquier otro script
    public static bool tieneLlave = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tieneLlave = true; // El jugador ahora tiene la llave
            Debug.Log("¡Llave recogida!");
            gameObject.SetActive(false); // La llave desaparece
        }
    }
}