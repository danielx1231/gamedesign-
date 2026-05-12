using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartHelper : MonoBehaviour
{
    public void RestartCurrentScene()
    {
        Time.timeScale = 1f;

        CountdownCheckpointData.ResetAll();

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void RestartToScene(string sceneName)
    {
        Time.timeScale = 1f;

        CountdownCheckpointData.ResetAll();

        SceneManager.LoadScene(sceneName);
    }
}