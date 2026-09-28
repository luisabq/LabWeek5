using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Starts the player and handles restart after death.
public class GameManager : MonoBehaviour
{
    // Player prefab spawned at game start.
    [SerializeField] private GameObject playerPrefab;

    // True once the player has died.
    public bool GameOver { get; private set; }

    // Listen for the death event.
    private void OnEnable() => GameEvents.OnPlayerDied += HandlePlayerDied;
    // Stop listening when disabled.
    private void OnDisable() => GameEvents.OnPlayerDied -= HandlePlayerDied;

    private void Start()
    {
        // Spawn the player and tell the rest of the game about it.
        GameObject player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        GameEvents.RaisePlayerSpawned(player.transform);
    }

    private void Update()
    {
        // Press R after game over to reload the scene.
        if (GameOver && Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Week5Lab");
        }
    }

    private void HandlePlayerDied()
    {
        // Locks the game into the restart state.
        GameOver = true;
    }
}