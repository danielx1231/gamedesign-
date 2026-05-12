using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class BGMController : MonoBehaviour
{
    public static BGMController Instance;

    [Header("Audio Clips")]
    public AudioClip beforePuzzleBGM;
    public AudioClip afterPuzzleBGM;

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    public float bgmVolume = 0.35f;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    private AudioSource audioSource;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        if (beforePuzzleBGM != null)
        {
            audioSource.clip = beforePuzzleBGM;
            audioSource.volume = bgmVolume;
            audioSource.Play();
        }
    }

    public void SwitchToAfterPuzzleBGM()
    {
        if (afterPuzzleBGM == null)
        {
            Debug.LogWarning("After puzzle BGM is not assigned.");
            return;
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(SwitchBGMWithFade(afterPuzzleBGM));
    }

    private IEnumerator SwitchBGMWithFade(AudioClip newClip)
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(bgmVolume, 0f, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();

        audioSource.clip = newClip;
        audioSource.time = 0f;
        audioSource.Play();

        t = 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(0f, bgmVolume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = bgmVolume;
        fadeCoroutine = null;
    }

    public void RewindBGM(float seconds)
    {
        if (audioSource == null) return;
        if (audioSource.clip == null) return;
        if (!audioSource.isPlaying) return;

        float newTime = audioSource.time - seconds;

        if (newTime < 0f)
        {
            newTime = 0f;
        }

        audioSource.time = newTime;
    }
}