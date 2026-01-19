using UnityEngine;

public class InteraccionLlave : MonoBehaviour
{
    public static bool teLaClau = false; 
    public GameObject indicatiuUI; // Arrastra aquí el "TextLlave" que acabas de crear
    private bool jugadorAProp = false;

    void Update()
    {
        // Si estamos cerca y pulsamos la E
        if (jugadorAProp && Input.GetKeyDown(KeyCode.E))
        {
            AgafarLlave();
        }
    }

    void AgafarLlave()
    {
        teLaClau = true;
        
        // Es vital ocultar el texto antes de que la llave desaparezca
        if (indicatiuUI != null) indicatiuUI.SetActive(false);
        
        Debug.Log("¡LLAVE RECOGIDA!");
        gameObject.SetActive(false); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorAProp = true;
            // Mostramos el aviso al acercarnos
            if (indicatiuUI != null) indicatiuUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorAProp = false;
            // Ocultamos el aviso al alejarnos
            if (indicatiuUI != null) indicatiuUI.SetActive(false);
        }
    }
}