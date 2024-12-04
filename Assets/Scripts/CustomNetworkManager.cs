using Unity.Netcode;
using UnityEngine;

public class CustomNetworkManager : MonoBehaviour
{
    public int connectedPlayers = 0; // Tracks the number of connected players

    private void Start()
    {
        if (NetworkManager.Singleton != null)
        {
            // Subscribe to the OnClientConnectedCallback event
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            // Unsubscribe from the events to avoid memory leaks
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        connectedPlayers++; // Increment the counter
        Debug.Log($"Player connected. Total players: {connectedPlayers}");
    }

    private void OnClientDisconnected(ulong clientId)
    {
        connectedPlayers--; // Decrement the counter
        Debug.Log($"Player disconnected. Total players: {connectedPlayers}");
    }
}
