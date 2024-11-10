using UnityEngine;
using Cinemachine;
using Unity.Netcode;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        // Check if the NetworkManager is available
        if (NetworkManager.Singleton != null)
        {
            // Subscribe to client connection events
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }

        // If the server starts (which includes the host), try to find the local player
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer)
        {
            SetCameraToLocalPlayer();
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        // Only set the camera for the local client
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            SetCameraToLocalPlayer();
        }
    }

    private void SetCameraToLocalPlayer()
    {
        // Find all player objects and set the camera to follow the local player
        foreach (var player in FindObjectsOfType<NetworkObject>())
        {
            if (player.IsOwner)
            {
                virtualCamera.Follow = player.transform;
                break;
            }
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events to avoid memory leaks
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }
}
