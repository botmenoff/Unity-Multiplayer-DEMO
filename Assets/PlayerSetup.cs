using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPun
{
    public Movement movement;
    public GameObject playerCamera;
    public TextMeshPro playerNameText;

    void Start()
    {
        // Verifica si este es el jugador local
        if (photonView.IsMine)
        {
            // Activa el movimiento y la cámara del jugador local
            movement.enabled = true;
            playerCamera.SetActive(true);
            // Configura el nombre del jugador en el texto del nombre
            SetPlayerName();
        }
        else
        {
            // Desactiva el movimiento y la cámara de los jugadores remotos
            movement.enabled = false;
            playerCamera.SetActive(false);
        }
    }

    void SetPlayerName()
    {
        // Verifica si el objeto de texto del nombre del jugador está asignado
        if (playerNameText != null)
        {
            // Muestra el nombre del jugador local
            playerNameText.text = GetRandomName();
        }
    }

    // Método para obtener un nombre aleatorio de la lista
    private string GetRandomName()
    {
        // Lista de nombres aleatorios
        string[] randomNames = { "Alex", "Bob", "Charlie", "David", "Emma", "Frank", "Grace", "Henry", "Ivy", "Jack", "Kate", "Leo", "Mia", "Noah", "Olivia" };
        // Selecciona un índice aleatorio de la lista de nombres
        int randomIndex = Random.Range(0, randomNames.Length);
        // Devuelve el nombre correspondiente al índice seleccionado
        return randomNames[randomIndex];
    }
}
