using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject completionPanel;
    public bool showCompletionOnPlayerEnter = false;
    public bool pauseWhenCompleted = true;
    public string playerTag = "Player";

    private bool isCompleted = false;

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void RestartCurrentScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        LoadScene("MainMenu");
    }

    public void ShowCompletionPanel()
    {
        if (isCompleted) return;
        isCompleted = true;

        if (completionPanel != null)
        {
            completionPanel.SetActive(true);
        }

        if (pauseWhenCompleted)
        {
            Time.timeScale = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!showCompletionOnPlayerEnter) return;
        if (!other.CompareTag(playerTag)) return;

        ShowCompletionPanel();
    }
}
