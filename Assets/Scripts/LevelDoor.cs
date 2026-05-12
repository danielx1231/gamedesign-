using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    [Header("Scene Settings")]
    public string targetSceneName = "Level3";

    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;

    [Header("UI Prompt")]
    public GameObject promptPanel;

    private bool playerInRange = false;

    private void Start()
    {
        if (promptPanel != null)
        {
            promptPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(interactKey))
        {
            LoadTargetScene();
        }
    }

    private void LoadTargetScene()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogWarning("Target Scene Name is empty.");
            return;
        }

        SceneManager.LoadScene(targetSceneName);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (promptPanel != null)
            {
                promptPanel.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (promptPanel != null)
            {
                promptPanel.SetActive(false);
            }
        }
    }
}