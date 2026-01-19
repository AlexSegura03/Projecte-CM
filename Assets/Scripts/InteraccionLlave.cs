using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InteraccionLlave : MonoBehaviour
{
    // Variables estàtiques (globals)
    public static bool teLaClau = false; 
    public static bool teLaClauFinal = false;

    [Header("Configuració clau")]
    public bool esLlaveDeVictoria = false;

    [Header("So")]
    [Tooltip("So que sona quan agafes la clau")]
    public AudioClip pickupSound;

    [Header("UI")]
    public GameObject indicatiuUI;

    private bool jugadorAProp = false;
    private bool recollida = false;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Configuració correcta del so
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 1f; // 3D real
        audioSource.volume = 1f;
    }

    void Update()
    {
        if (jugadorAProp && !recollida && Input.GetKeyDown(KeyCode.E))
        {
            AgafarLlave();
        }
    }

    void AgafarLlave()
    {
        recollida = true;

        if (esLlaveDeVictoria)
            teLaClauFinal = true;
        else
            teLaClau = true;

        if (indicatiuUI != null)
            indicatiuUI.SetActive(false);

        Debug.Log("LLAVE RECOGIDA. Final?: " + esLlaveDeVictoria);

        // Reprodueix el so
        if (pickupSound != null)
        {
            audioSource.clip = pickupSound;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("InteraccionLlave: no s'ha assignat cap AudioClip");
        }

        // Amaga la clau (sense matar el so)
        if (TryGetComponent(out Collider col))
            col.enabled = false;

        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = false;

        // Destrueix l'objecte quan acabi el so
        float delay = pickupSound != null ? pickupSound.length : 0f;
        Destroy(gameObject, delay);
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
