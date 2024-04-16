using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Movement : MonoBehaviourPun
{
    public float walkSpeed = 4f; // Velocidad de movimiento del jugador
    public float maxVelocityChange = 10f; // Cambio máximo de velocidad permitido

    private Vector2 input; // Entrada de movimiento del jugador
    private Rigidbody rb; // Referencia al componente Rigidbody del jugador

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtiene el componente Rigidbody del jugador

        // Desactiva el componente Rigidbody si no es el jugador local
        if (!photonView.IsMine)
        {
            Destroy(rb);
        }
    }

    void Update()
    {
        // Solo procesa la entrada de movimiento si es el jugador local
        if (photonView.IsMine)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // Obtiene la entrada de movimiento del jugador
            input.Normalize(); // Normaliza el vector de entrada para que tenga una magnitud de 1 si se mueve en diagonal
        }
    }

    void FixedUpdate()
    {
        // Solo aplica el movimiento si es el jugador local
        if (photonView.IsMine)
        {
            // Calcula el cambio de velocidad del jugador
            Vector3 velocityChange = CalculateMovement(walkSpeed);
            // Aplica el cambio de velocidad al Rigidbody del jugador
            rb.AddForce(velocityChange, ForceMode.VelocityChange);

            // Sincroniza la posición del jugador a través de la red
            photonView.RPC("SyncPosition", RpcTarget.Others, transform.position);
        }
    }

    [PunRPC]
    void SyncPosition(Vector3 newPosition)
    {
        // Actualiza la posición del jugador en todos los clientes excepto el jugador local
        if (!photonView.IsMine)
        {
            transform.position = newPosition;
        }
    }

    // Método para calcular el cambio de velocidad del jugador
    Vector3 CalculateMovement(float _speed)
    {
        Vector3 targetVelocity = new Vector3(input.x, 0, input.y); // Calcula la velocidad objetivo basada en la entrada del jugador
        targetVelocity = transform.TransformDirection(targetVelocity); // Transforma la velocidad objetivo a la dirección local del jugador

        targetVelocity *= _speed; // Escala la velocidad objetivo por la velocidad de movimiento del jugador

        Vector3 velocity = rb.velocity; // Obtiene la velocidad actual del jugador
        Vector3 velocityChange = Vector3.zero; // Inicializa el cambio de velocidad como cero

        if (input.magnitude > 0.5f) // Verifica si hay entrada de movimiento significativa
        {
            velocityChange = targetVelocity - velocity; // Calcula el cambio de velocidad requerido para alcanzar la velocidad objetivo

            // Limita el cambio de velocidad en cada eje para evitar movimientos bruscos
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);

            velocityChange.y = 0; // No cambia la velocidad en el eje Y (evita saltos)
        }

        return velocityChange; // Devuelve el cambio de velocidad calculado
    }
}