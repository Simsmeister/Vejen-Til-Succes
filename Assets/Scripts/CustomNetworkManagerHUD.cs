using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CustomNetworkManagerHUD : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;
    public Button serverButton;

    public GameObject startUI;

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
        CloseCanvas();
    }

    private void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        CloseCanvas();
    }

    private void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        CloseCanvas();
    }

    private void CloseCanvas()
    {
        startUI.SetActive(false);
    }
}
