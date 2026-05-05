using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CountdownChallengeManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject popupPanel;
    public TextMeshProUGUI countdownText;

    [Header("Timer")]
    public float countdownDuration = 179f; // 2 minutes 59 seconds
    public float currentTime;

    private bool isPopupShowing = false;
    private bool isCountingDown = false;
    private float inputDelay = 0.2f;

    void Start()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isPopupShowing)
        {
            inputDelay -= Time.unscaledDeltaTime;

            if (inputDelay <= 0f && Input.anyKeyDown)
            {
                ClosePopupAndStartCountdown();
            }

            return;
        }

        if (isCountingDown)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0f)
            {
                currentTime = 0f;
                UpdateTimerUI();
                RestartLevel();
                return;
            }

            UpdateTimerUI();
        }
    }

    public void ShowCountdownPopup()
    {
        isPopupShowing = true;
        inputDelay = 0.2f;

        Time.timeScale = 0f;

        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
        }

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }

    private void ClosePopupAndStartCountdown()
    {
        isPopupShowing = false;

        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        Time.timeScale = 1f;

        StartCountdown();
    }

    private void StartCountdown()
    {
        currentTime = countdownDuration;
        isCountingDown = true;

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }

        UpdateTimerUI();

        if (BGMController.Instance != null)
        {
            BGMController.Instance.SwitchToAfterPuzzleBGM();
        }
        else
        {
            Debug.LogWarning("BGMController.Instance is NULL. Countdown starts without BGM switch.");
        }
    }

    public void AddTime(float amount)
    {
        if (!isCountingDown)
        {
            return;
        }

        currentTime += amount;

        if (currentTime > countdownDuration)
        {
            currentTime = countdownDuration;
        }

        UpdateTimerUI();

        if (BGMController.Instance != null)
        {
            BGMController.Instance.RewindBGM(amount);
        }
        else
        {
            Debug.LogWarning("BGMController.Instance is NULL. Cannot rewind BGM.");
        }
    }

    private void UpdateTimerUI()
    {
        if (countdownText == null)
        {
            return;
        }

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        countdownText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    private void RestartLevel()
    {
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}