using UnityEngine;
using TMPro; // Make sure to include this namespace for TextMeshPro

/// <summary>
/// Manages the UI elements, like the note display panel and look prompts.
/// Implemented as a Singleton to be easily accessible.
/// </summary>
public class UIManager : MonoBehaviour
{
    // Singleton instance
    public static UIManager Instance { get; private set; }

    [Header("Note Panel UI")]
    [SerializeField] private GameObject noteDisplayPanel;
    [SerializeField] private TextMeshProUGUI noteText;

    [Header("Look Prompt UI")]
    [SerializeField] private GameObject lookPromptPanel;
    [SerializeField] private TextMeshProUGUI lookPromptText;


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

        // Ensure UI is hidden on start
        HideInteractionPanel();
        HideLookPrompt();
    }

    // --- Note Panel Methods ---

    /// <summary>
    /// Shows the main note display panel with the provided text.
    /// </summary>
    /// <param name="textToShow">The string to display in the note panel.</param>
    public void ShowInteractionPanel(string textToShow)
    {
        if (noteDisplayPanel != null && noteText != null)
        {
            noteText.text = textToShow;
            noteDisplayPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Hides the main note display panel.
    /// </summary>
    public void HideInteractionPanel()
    {
        if (noteDisplayPanel != null)
        {
            noteDisplayPanel.SetActive(false);
        }
    }

    // --- Look Prompt Methods ---

    /// <summary>
    /// Shows the small "look at" prompt with the provided text.
    /// </summary>
    /// <param name="promptText">The string to display in the look prompt.</param>
    public void ShowLookPrompt(string promptText)
    {
        if (lookPromptPanel != null && lookPromptText != null)
        {
            lookPromptText.text = promptText;
            lookPromptPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Hides the "look at" prompt.
    /// </summary>
    public void HideLookPrompt()
    {
        if (lookPromptPanel != null)
        {
            lookPromptPanel.SetActive(false);
        }
    }
}
