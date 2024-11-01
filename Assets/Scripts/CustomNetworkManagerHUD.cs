using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CustomNetworkManagerHUD : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;
    public Button serverButton;

    void Start()
    {
        // Assign button click events
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
        serverButton.onClick.AddListener(StartServer);
    }

    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    private void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    private void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }
}
