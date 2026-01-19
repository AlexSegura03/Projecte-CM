using UnityEngine;

public class InteraccionLlave : MonoBehaviour
{
    private bool estaCerca = false;

    private void OnTriggerEnter(Collider other)
    {
        // Si el jugador entra en la zona de la llave
        if (other.CompareTag("Player"))
        {
            estaCerca = true;
            Debug.Log("Pulsa E para recoger la llave");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Si el jugador se aleja de la llave
        if (other.CompareTag("Player"))
        {
            estaCerca = false;
        }
    }

    private void Update()
    {
        // Si estamos cerca y pulsamos la tecla E
        if (estaCerca && Input.GetKeyDown(KeyCode.E))
        {
            Recoger();
        }
    }

    void Recoger()
    {
        // Aquí activamos la variable global de la llave (que hicimos ayer)
        // O simplemente hacemos que la llave desaparezca
        Debug.Log("¡Llave recogida con la E!");
        gameObject.SetActive(false); 
    }
}