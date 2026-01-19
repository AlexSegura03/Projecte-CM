using UnityEngine;
using TMPro; // Añadimos esto para controlar el texto de TextMeshPro

public class Porta : MonoBehaviour
{
    public TextMeshProUGUI textoUI; // Arrastra aquí el componente de texto del Canvas
    private bool jugadorAProp = false;

    // Dentro del Update del script Porta
    void Update()
    {
        if (jugadorAProp)
        {
            // IMPORTANTE: Aquí debe poner el nombre exacto del script de la llave
            if (InteraccionLlave.teLaClau) 
            {
                textoUI.text = "Prem E per passar";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    ObrirPorta();
                }
            }
            else
            {
                textoUI.text = "Has de trobar la clau";
            }
        }
    }
    void Start()
    {
        // Al empezar el nivel, aseguramos que la llave no esté recogida
        InteraccionLlave.teLaClau = false;
    }
    public void ObrirPorta()
    {
        textoUI.gameObject.SetActive(false); // Ocultamos el texto
        Destroy(gameObject); // Destruimos la puerta
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorAProp = true;
            textoUI.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorAProp = false;
            textoUI.gameObject.SetActive(false);
        }
    }
}