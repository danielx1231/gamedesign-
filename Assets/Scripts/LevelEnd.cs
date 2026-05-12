using UnityEngine;
using UnityEngine.SceneManagement; // Required for switching levels

public class LevelExit : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject _interactionUI; // The Textbox/Canvas object
    [SerializeField] private string _nextLevelName;      // Name of the scene to load

    private bool _isPlayerInZone = false;

    private void Start()
    {
        // Ensure the UI is hidden when the level starts
        if (_interactionUI != null)
            _interactionUI.SetActive(false);
    }

    private void Update()
    {
        // Check if the player is in the zone AND presses 'E'
        if (_isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            LoadNextLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _isPlayerInZone = true;
            if (_interactionUI != null) _interactionUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _isPlayerInZone = false;
            if (_interactionUI != null) _interactionUI.SetActive(false);
        }
    }

    private void LoadNextLevel()
    {
        // Make sure you have added your scenes to Build Settings!
        SceneManager.LoadScene(_nextLevelName);
    }
}