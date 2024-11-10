using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private List<GameObject> characterPrefabs; // Assign character prefabs in Inspector
    private List<int> availableCharacters = new List<int>();

    private void Start()
    {
        // Initialize the list of available character indices
        for (int i = 0; i < characterPrefabs.Count; i++)
        {
            availableCharacters.Add(i);
        }

        // Subscribe to client connection events
        NetworkManager.Singleton.OnClientConnectedCallback += AssignCharacter;
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= AssignCharacter;
        }
    }

    private void AssignCharacter(ulong clientId)
    {
        if (availableCharacters.Count == 0)
        {
            Debug.LogWarning("No more available characters to assign.");
            return;
        }

        // Randomly select an index from available characters
        int randomIndex = Random.Range(0, availableCharacters.Count);
        int characterIndex = availableCharacters[randomIndex];

        // Remove the chosen character index from available list
        availableCharacters.RemoveAt(randomIndex);

        // Assign character to player
        AssignCharacterToPlayerServerRpc(clientId, characterIndex);
    }

    [ServerRpc(RequireOwnership = false)]
    private void AssignCharacterToPlayerServerRpc(ulong clientId, int characterIndex)
    {
        // Instantiate the character for the player and assign ownership to the client
        GameObject characterInstance = Instantiate(characterPrefabs[characterIndex]);
        characterInstance.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
    }
}
