using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Movement : MonoBehaviourPun
{
    public float walkSpeed = 4f;
    public float maxVelocityChange = 10f;

    private Vector2 input;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

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
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            input.Normalize();
        }
    }

    void FixedUpdate()
    {
        // Solo aplica el movimiento si es el jugador local
        if (photonView.IsMine)
        {
            Vector3 velocityChange = CalculateMovement(walkSpeed);
            rb.AddForce(velocityChange, ForceMode.VelocityChange);

            // Sincroniza la posición del jugador a través de la red
            photonView.RPC("SyncPosition", RpcTarget.Others, transform.position);
        }
    }

    [PunRPC]
    void SyncPosition(Vector3 newPosition)
    {
        // Actualiza la posición del jugador en todos los clientes
        if (!photonView.IsMine)
        {
            transform.position = newPosition;
        }
    }

    Vector3 CalculateMovement(float _speed)
    {
        Vector3 targetVelocity = new Vector3(input.x, 0, input.y);
        targetVelocity = transform.TransformDirection(targetVelocity);

        targetVelocity *= _speed;

        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = Vector3.zero;

        if (input.magnitude > 0.5f)
        {
            velocityChange = targetVelocity - velocity;

            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);

            velocityChange.y = 0;
        }

        return velocityChange;
    }
}
