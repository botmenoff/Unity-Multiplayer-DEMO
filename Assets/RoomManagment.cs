using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomManagment : MonoBehaviourPunCallbacks
{
    // Variables públicas para el jugador y el punto de aparición
    public GameObject player;
    public Transform spawnPoint;


    // Lista de nombres aleatorios
    private string[] randomNames = { "Alex", "Bob", "Charlie", "David", "Emma", "Frank", "Grace", "Henry", "Ivy", "Jack", "Kate", "Leo", "Mia", "Noah", "Olivia" };

    // Método Start, se llama cuando se inicializa el objeto
    void Start()
    {
        // Mensaje de depuración para indicar que se está conectando
        Debug.Log("Conectando....");
        // Conecta al servidor Photon utilizando la configuración establecida en el editor
        PhotonNetwork.ConnectUsingSettings();
    }

    // Método invocado cuando se ha conectado al servidor Photon
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        // Mensaje de depuración para indicar que se ha conectado al servidor
        Debug.Log("Conectado al servidor");
        // Se une al lobby para poder ver o crear salas
        PhotonNetwork.JoinLobby();
    }

    // Método invocado cuando se ha unido al lobby de Photon
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        // Mensaje de depuración para indicar que se ha unido a un lobby
        Debug.Log("Estamos conectados a una Sala");
        // Se une a una sala existente con el nombre "test" o la crea si no existe
        PhotonNetwork.JoinOrCreateRoom("test", null, null);
    }

    // Método invocado cuando se ha unido a una sala de Photon
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        // Mensaje de depuración para indicar que se ha unido a la sala
        Debug.Log("Se ha unido a la sala");
        SpawnPlayer();
    }

    // Método invocado cuando se ha creado una nueva sala de Photon
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        // Mensaje de depuración para indicar que se ha creado la sala
        Debug.Log("Se ha creado la sala");
        SpawnPlayer();
    }

    // Método para obtener un nombre aleatorio de la lista
    private string GetRandomName()
    {
        // Selecciona un índice aleatorio de la lista de nombres
        int randomIndex = Random.Range(0, randomNames.Length);
        // Devuelve el nombre correspondiente al índice seleccionado
        return randomNames[randomIndex];
    }

    // Método para instanciar al jugador en el punto de aparición
    private void SpawnPlayer()
    {
        string playerName = GetRandomName();
        if (player != null)
        {
            // Obtiene el punto de aparición desde el campo del inspector
            Transform spawnTransform = spawnPoint != null ? spawnPoint : transform;
            PhotonNetwork.Instantiate(player.name, spawnTransform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Player prefab is not assigned.");
        }
    }
}
