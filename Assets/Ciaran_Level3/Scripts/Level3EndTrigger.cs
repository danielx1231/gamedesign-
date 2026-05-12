using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level3EndTrigger : MonoBehaviour
{
    [SerializeField] private GameObject promptRoot;
    [SerializeField] private Text promptText;
    [SerializeField] private GameObject completionPanel;
    [SerializeField] private string promptMessage = "Press E to complete Level 3";
    [SerializeField] private string playerTag = "Player";

    private bool isPlayerInRange;
    private bool hasCompleted;

    private void Start()
    {
        SetPromptVisible(false);

        if (completionPanel != null)
        {
            completionPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (hasCompleted || !isPlayerInRange)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        if (hasCompleted)
        {
            return;
        }

        hasCompleted = true;
        isPlayerInRange = false;

        SetPromptVisible(false);

        if (completionPanel != null)
        {
            completionPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void RestartCurrentScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasCompleted || !IsPlayer(other))
        {
            return;
        }

        isPlayerInRange = true;

        if (promptText != null)
        {
            promptText.text = promptMessage;
        }

        SetPromptVisible(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsPlayer(other))
        {
            return;
        }

        isPlayerInRange = false;

        if (!hasCompleted)
        {
            SetPromptVisible(false);
        }
    }

    private bool IsPlayer(Collider2D other)
    {
        return other.CompareTag(playerTag) ||
               other.gameObject.name.StartsWith("player", StringComparison.OrdinalIgnoreCase);
    }

    private void SetPromptVisible(bool visible)
    {
        if (promptRoot != null)
        {
            promptRoot.SetActive(visible);
            return;
        }

        if (promptText != null)
        {
            promptText.gameObject.SetActive(visible);
        }
    }
}
