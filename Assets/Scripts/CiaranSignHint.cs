using UnityEngine;
using TMPro;

public class CiaranSignHint : MonoBehaviour
{
    [Header("Player Detection")]
    public string playerTag = "Player";

    [Header("Hint UI")]
    public GameObject hintPanel;
    public TMP_Text hintText;

    [TextArea(2, 5)]
    public string hintMessage = "This seems to be related to \"cherry pie\"... If you cannot figure it out, the mechanism near the entrance can help you start over.";

    private void Start()
    {
        if (hintPanel != null)
        {
            hintPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            ShowHint();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            HideHint();
        }
    }

    private void ShowHint()
    {
        if (hintText != null)
        {
            hintText.text = hintMessage;
        }

        if (hintPanel != null)
        {
            hintPanel.SetActive(true);
        }
    }

    private void HideHint()
    {
        if (hintPanel != null)
        {
            hintPanel.SetActive(false);
        }
    }
}