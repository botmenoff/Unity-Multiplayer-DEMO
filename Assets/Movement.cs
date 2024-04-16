using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float walkSpeed = 4f; // Velocidad de movimiento del jugador
    public float sprintSpeed = 8f; // Velocidad de esprintar del jugador
    public float maxVelocityChange = 10f; // Cambio máximo de velocidad permitido
    public float jumpForce = 10f; // Fuerza de salto del jugador

    private Rigidbody rb; // Referencia al componente Rigidbody del jugador
    private bool isSprinting = false; // Flag para indicar si el jugador está esprintando
    private bool isGrounded = false; // Flag para indicar si el jugador está en el suelo

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtiene el componente Rigidbody del jugador
    }

    void Update()
    {
        // Detecta la entrada de esprintar
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        // Detecta la entrada de saltar si el jugador está en el suelo
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Calcula la velocidad objetivo basada en la entrada del jugador
        float speed = isSprinting ? sprintSpeed : walkSpeed;
        Vector3 targetVelocity = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * speed;

        // Calcula el cambio de velocidad del jugador
        Vector3 velocityChange = targetVelocity - rb.velocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        // Aplica el cambio de velocidad al Rigidbody del jugador
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void Jump()
    {
        // Aplica una fuerza hacia arriba al Rigidbody para simular el salto
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionStay(Collision collision)
    {
        // Verifica si el jugador está en el suelo al entrar en contacto con un objeto
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        // Verifica si el jugador ha dejado de estar en el suelo
        isGrounded = false;
    }
}