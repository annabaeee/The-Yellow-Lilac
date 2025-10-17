using UnityEngine;
using TMPro; // Make sure to include this namespace for TextMeshPro

/// <summary>
/// Manages the UI elements, like the note display panel.
/// Implemented as a Singleton to be easily accessible.
/// </summary>
public class UIManager : MonoBehaviour
{
    // Singleton instance
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject noteDisplayPanel;
    [SerializeField] private TextMeshProUGUI noteText;

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Shows the note display panel with the provided text.
    /// </summary>
    /// <param name="textToShow">The string to display in the text element.</param>
    public void ShowInteractionPanel(string textToShow)
    {
        if (noteDisplayPanel != null && noteText != null)
        {
            noteText.text = textToShow;
            noteDisplayPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Hides the note display panel.
    /// </summary>
    public void HideInteractionPanel()
    {
        if (noteDisplayPanel != null)
        {
            noteDisplayPanel.SetActive(false);
        }
    }
}
