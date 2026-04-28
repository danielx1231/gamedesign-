using UnityEngine;
using System.Collections;

public class BGMController : MonoBehaviour
{
    public static BGMController Instance;

    [Header("Audio")]
    public AudioSource musicSource;
    public AudioClip bgmBeforePuzzle;
    public AudioClip bgmAfterPuzzle;

    [Header("Fade")]
    public float fadeOutTime = 1.2f;
    public float fadeInTime = 1.2f;
    public float targetVolume = 0.35f;

    private Coroutine routine;

    private void Awake()
    {
        Instance = this; // ✅ 直接覆盖式赋值，保证不为null（单场景用很稳）

        if (musicSource == null)
            musicSource = GetComponent<AudioSource>();

        // 推荐由脚本控制播放，避免Inspector PlayOnAwake打架
        if (musicSource != null)
            musicSource.playOnAwake = false;
    }

    private void Start()
    {
        PlayInitialBGM();
    }

    private void PlayInitialBGM()
    {
        if (musicSource == null || bgmBeforePuzzle == null) return;

        musicSource.clip = bgmBeforePuzzle;
        musicSource.loop = true;
        musicSource.volume = targetVolume;
        musicSource.Play();
    }

    public void SwitchToAfterPuzzleBGM()
    {
        if (musicSource == null || bgmAfterPuzzle == null) return;

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FadeSwitch(bgmAfterPuzzle));
    }

    private IEnumerator FadeSwitch(AudioClip newClip)
    {
        // Fade out
        float startVol = musicSource.volume;
        float t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVol, 0f, t / fadeOutTime);
            yield return null;
        }
        musicSource.volume = 0f;

        // Switch
        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.loop = true;
        musicSource.Play();

        // Fade in
        t = 0f;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, targetVolume, t / fadeInTime);
            yield return null;
        }
        musicSource.volume = targetVolume;
    }
}