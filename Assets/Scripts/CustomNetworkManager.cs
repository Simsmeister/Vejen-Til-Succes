using Unity.Netcode;
using UnityEngine;

public class CustomNetworkManager : MonoBehaviour
{
    public GameObject playerPrefab;

    private void Start()
    {
        // Subscribe to events when the network starts
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
    }

    private void OnServerStarted()
    {
        // Only execute this if we're a dedicated server (not a host)
        if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsHost)
        {
            Debug.Log("Server started as a dedicated server.");
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        // Only spawn the player for clients, including the host's client instance
        if (NetworkManager.Singleton.IsServer)
        {
            // Check if this is the local client ID for the host
            if (clientId == NetworkManager.Singleton.LocalClientId && NetworkManager.Singleton.IsHost)
            {
                Debug.Log("Spawning player for the host client.");
            }
            else
            {
                Debug.Log($"Spawning player for client {clientId}.");
            }
            SpawnPlayer(clientId);
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        // Instantiate the player object and assign ownership to the connected client
        GameObject playerInstance = Instantiate(playerPrefab);
        playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }

    private void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
        }
    }
}
