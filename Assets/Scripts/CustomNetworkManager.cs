using Unity.Netcode;
using UnityEngine;

public class CustomNetworkManager : MonoBehaviour
{
    // Synchronized NetworkVariable to track the number of connected players
    public NetworkVariable<int> connectedPlayers = new NetworkVariable<int>(
    NetworkVariableReadPermission.Everyone, // All clients can read this value
    NetworkVariableWritePermission.Server   // Only the server can modify this value
);

    private void Start()
    {
        if (NetworkManager.Singleton != null)
        {
            // Subscribe to connection and disconnection events
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;

            // Subscribe to NetworkVariable changes
            connectedPlayers.OnValueChanged += OnConnectedPlayersChanged;
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            // Unsubscribe from events
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;

            // Unsubscribe from NetworkVariable changes
            connectedPlayers.OnValueChanged -= OnConnectedPlayersChanged;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer) // Ensure only the server modifies the value
        {
            connectedPlayers.Value++; // Increment the count
            Debug.Log($"Player connected. Total players: {connectedPlayers.Value}");
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer) // Ensure only the server modifies the value
        {
            connectedPlayers.Value--; // Decrement the count
            Debug.Log($"Player disconnected. Total players: {connectedPlayers.Value}");
        }
    }

    private void OnConnectedPlayersChanged(int previousValue, int newValue)
    {
        // Triggered when the connectedPlayers variable changes
        Debug.Log($"Connected players updated: {newValue}");
    }
}
