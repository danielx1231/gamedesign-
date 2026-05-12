using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CountdownChallengeManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject popupPanel;
    public TMP_Text countdownText;

    [Header("Timer")]
    public float countdownDuration = 179f;
    private float currentTime;
    private bool countdownStarted = false;
    private bool waitingForInput = false;

    [Header("Checkpoint")]
    public Transform player;
    public Transform checkpointPoint;

    [Header("BGM")]
    public AudioSource countdownBGMSource;

    private void Start()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        // 如果是倒计时结束 / 死亡后重新加载场景
        // 直接从挑战开始状态恢复，不再显示弹窗
        if (CountdownCheckpointData.shouldStartChallengeOnLoad)
        {
            CountdownCheckpointData.ConsumeChallengeStartFlag();
            StartCountdownImmediatelyAfterReload();
        }
    }

    private void Update()
    {
        if (waitingForInput)
        {
            if (Input.anyKeyDown)
            {
                StartCountdown();
            }
        }

        if (countdownStarted)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0f)
            {
                currentTime = 0f;
                UpdateCountdownUI();
                RestartFromCheckpoint();
                return;
            }

            UpdateCountdownUI();
        }
    }

    public void ShowCountdownPopup()
    {
        if (checkpointPoint != null)
        {
            CountdownCheckpointData.SaveCheckpoint(checkpointPoint.position);
        }
        else if (player != null)
        {
            CountdownCheckpointData.SaveCheckpoint(player.position);
        }

        CountdownCheckpointData.UnlockChallenge();

        waitingForInput = true;
        countdownStarted = false;

        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
        }

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }

    private void StartCountdown()
    {
        waitingForInput = false;
        countdownStarted = true;
        currentTime = countdownDuration;

        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }

        UpdateCountdownUI();
        StartCountdownBGMFromBeginning();
    }

    private void StartCountdownImmediatelyAfterReload()
    {
        waitingForInput = false;
        countdownStarted = true;
        currentTime = countdownDuration;

        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }

        UpdateCountdownUI();
        StartCountdownBGMFromBeginning();

        Debug.Log("Countdown restarted from checkpoint state.");
    }

    private void StartCountdownBGMFromBeginning()
    {
        // 如果你原来的 BGMController 负责切换倒计时音乐，保留这段
        if (BGMController.Instance != null)
        {
            BGMController.Instance.SwitchToAfterPuzzleBGM();
        }

        // 如果你拖了 AudioSource，这里会强制从音乐开头播放
        if (countdownBGMSource != null)
        {
            countdownBGMSource.time = 0f;

            if (!countdownBGMSource.isPlaying)
            {
                countdownBGMSource.Play();
            }
        }
    }

    private void RestartFromCheckpoint()
    {
        CountdownCheckpointData.PrepareChallengeRestart();

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void UpdateCountdownUI()
    {
        if (countdownText == null) return;

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        countdownText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    // 给 ClockPickup 用：获得时钟后增加时间
    public void AddTime(float amount)
    {
        currentTime += amount;

        // 音乐回退 amount 秒
        if (countdownBGMSource != null)
        {
            countdownBGMSource.time = Mathf.Max(0f, countdownBGMSource.time - amount);
        }

        UpdateCountdownUI();
    }

    // 保留这个方法名，防止你之前有其他脚本调用它
    public void AddTimeAndRewindMusic(float amount)
    {
        AddTime(amount);
    }
}