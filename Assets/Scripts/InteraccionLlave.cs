using UnityEngine;

public class InteraccionLlave : MonoBehaviour
{
    // Variables estáticas (globales)
    public static bool teLaClau = false; 
    public static bool teLaClauFinal = false; // Nueva variable para la victoria

    [Header("Configuración")]
    public bool esLlaveDeVictoria = false; // ¡Marca esto en el Inspector para la segunda llave!
    
    public GameObject indicatiuUI; 
    private bool jugadorAProp = false;

    void Update()
    {
        if (jugadorAProp && Input.GetKeyDown(KeyCode.E))
        {
            AgafarLlave();
        }
    }

    void AgafarLlave()
    {
        // Si es la llave de victoria, activamos su propia variable
        if (esLlaveDeVictoria) 
        {
            teLaClauFinal = true;
        }
        else 
        {
            teLaClau = true;
        }
        
        if (indicatiuUI != null) indicatiuUI.SetActive(false);
        Debug.Log("¡LLAVE RECOGIDA! ¿Es final?: " + esLlaveDeVictoria);
        gameObject.SetActive(false); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorAProp = true;
            if (indicatiuUI != null) indicatiuUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorAProp = false;
            if (indicatiuUI != null) indicatiuUI.SetActive(false);
        }
    }
}