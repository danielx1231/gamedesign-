using UnityEngine;

public class SignPuzzleClue : MonoBehaviour
{
    private AudioSource audioSource;
    private bool isPlayerInRange = false;

    private string clueMessage = "This seems to be connected to \"cherry pie\"... If you get stuck, the mechanism near the entrance may help you reset the puzzle.";

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("The object " + gameObject.name + " is missing an AudioSource component!");
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PlayClueMusic();
        }
    }

    private void PlayClueMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        Debug.Log(clueMessage);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Press E to check the clue.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}