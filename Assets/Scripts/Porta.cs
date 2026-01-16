using UnityEngine;

public class Porta : MonoBehaviour
{
    // Ja no cal configurar angles ni velocitats

    public void ObrirPorta()
    {
        // Simplement fem que l'objecte es desactivi (desapareix)
        //gameObject.SetActive(false);
        
        // Si preferíssiu destruir-lo del tot (esborrar-lo de la memòria):
        Destroy(gameObject);
    }
}