using UnityEngine;

public class SignHint : MonoBehaviour
{
    [Header("UI")]
    public GameObject hintPanel;

    private void Start()
    {
        if (hintPanel != null)
        {
            hintPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShowPanel();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HidePanel();
        }
    }

    private void ShowPanel()
    {
        if (hintPanel != null)
        {
            hintPanel.SetActive(true);
        }
    }

    private void HidePanel()
    {
        if (hintPanel != null)
        {
            hintPanel.SetActive(false);
        }
    }
}