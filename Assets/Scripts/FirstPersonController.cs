using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Header("Moviment")]
    public CharacterController controller;
    public float velocitatMoviment = 12f;
    public float velocitatSalt = 3f;
    public float gravetat = -9.81f;

    [Header("Terra")]
    public Transform groundCheck;
    public float distanciaTerra = 0.4f;
    public LayerMask mascaraTerra;

    [Header("Vista")]
    public float sensibilitatRatoli = 200f;
    public Camera cameraPOV;

    [Header("Llanterna")]
    public Transform llanternaPivot;
    public Light llanternaLight;
    public KeyCode teclaLlanterna = KeyCode.F;

    [Header("Efectes Llanterna")]
    public float retardRotacio = 8f;
    public float intensitatBase = 3f;
    public float flickerVelocitat = 15f;
    public float flickerIntensitat = 0.3f;
    public float tremolorRotacio = 0.5f;
    public float balanceigMoviment = 1.5f;

    Vector3 velocitatVertical;
    bool estaAlTerra;
    float rotacioVertical = 0f;
    bool llanternaActiva = true;

    Quaternion rotacioObjectiu;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraPOV == null)
            cameraPOV = Camera.main;

        if (llanternaLight != null)
            intensitatBase = llanternaLight.intensity;
    }

    void Update()
    {
        ComprovarTerra();
        Moviment();
        SaltIGravetat();
        Vista();
        Llanterna();
    }

    void ComprovarTerra()
    {
        estaAlTerra = Physics.CheckSphere(groundCheck.position, distanciaTerra, mascaraTerra);

        if (estaAlTerra && velocitatVertical.y < 0)
            velocitatVertical.y = -2f;
    }

    void Moviment()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moviment = transform.right * x + transform.forward * z;
        controller.Move(moviment * velocitatMoviment * Time.deltaTime);
    }

    void SaltIGravetat()
    {
        if (Input.GetButtonDown("Jump") && estaAlTerra)
            velocitatVertical.y = Mathf.Sqrt(velocitatSalt * -2f * gravetat);

        velocitatVertical.y += gravetat * Time.deltaTime;
        controller.Move(velocitatVertical * Time.deltaTime);
    }

    void Vista()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilitatRatoli * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilitatRatoli * Time.deltaTime;

        rotacioVertical -= mouseY;
        rotacioVertical = Mathf.Clamp(rotacioVertical, -90f, 90f);

        cameraPOV.transform.localRotation = Quaternion.Euler(rotacioVertical, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void Llanterna()
    {
        if (llanternaPivot == null || llanternaLight == null)
            return;

        // Encesa / apagada
        if (Input.GetKeyDown(teclaLlanterna))
        {
            llanternaActiva = !llanternaActiva;
            llanternaLight.enabled = llanternaActiva;
        }

        if (!llanternaActiva)
            return;

        // Rotació objectiu (vista)
        rotacioObjectiu = Quaternion.Euler(
            cameraPOV.transform.eulerAngles.x,
            transform.eulerAngles.y,
            0f
        );

        // Tremolor
        Quaternion tremolor = Quaternion.Euler(
            Random.Range(-tremolorRotacio, tremolorRotacio),
            Random.Range(-tremolorRotacio, tremolorRotacio),
            0f
        );

        // Suavitzat + inèrcia
        llanternaPivot.rotation = Quaternion.Slerp(
            llanternaPivot.rotation,
            rotacioObjectiu * tremolor,
            retardRotacio * Time.deltaTime
        );

        // Balanceig amb el moviment
        float moviment = controller.velocity.magnitude;
        float balanceig = Mathf.Sin(Time.time * 6f) * moviment * balanceigMoviment;

        llanternaPivot.localRotation *= Quaternion.Euler(balanceig, 0f, 0f);

        // Flickering
        float flicker = Mathf.PerlinNoise(Time.time * flickerVelocitat, 0f);
        llanternaLight.intensity = intensitatBase + (flicker - 0.5f) * flickerIntensitat;
    }
}
