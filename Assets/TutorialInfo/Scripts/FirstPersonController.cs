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
    public float sensibilitatRatoli = 100f;
    public Camera cameraPOV;

    Vector3 velocitatVertical;
    bool estaAlTerra;
    float rotacioVertical = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraPOV == null)
            cameraPOV = Camera.main;
    }

    void Update()
    {
        // Terra
        estaAlTerra = Physics.CheckSphere(groundCheck.position, distanciaTerra, mascaraTerra);

        if (estaAlTerra && velocitatVertical.y < 0)
            velocitatVertical.y = -2f;

        // Moviment
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moviment = transform.right * x + transform.forward * z;
        controller.Move(moviment * velocitatMoviment * Time.deltaTime);

        // Salt
        if (Input.GetButtonDown("Jump") && estaAlTerra)
            velocitatVertical.y = Mathf.Sqrt(velocitatSalt * -2f * gravetat);

        // Gravetat
        velocitatVertical.y += gravetat * Time.deltaTime;
        controller.Move(velocitatVertical * Time.deltaTime);

        // Ratolí
        float mouseX = Input.GetAxis("Mouse X") * sensibilitatRatoli * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilitatRatoli * Time.deltaTime;

        rotacioVertical -= mouseY;
        rotacioVertical = Mathf.Clamp(rotacioVertical, -90f, 90f);

        cameraPOV.transform.localRotation = Quaternion.Euler(rotacioVertical, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
