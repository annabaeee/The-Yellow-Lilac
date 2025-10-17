using UnityEngine;

/// <summary>
/// Attach this script to any GameObject you want to be interactable.
/// It holds the text for display and a reference to its highlight outline.
/// </summary>
public class InteractableObject : MonoBehaviour
{
    [Header("UI Display")]
    [Tooltip("The text that will be displayed in the UI panel.")]
    [TextArea(5, 10)]
    public string interactionText = "Default interaction text.";

    [Header("Visuals")]
    [Tooltip("The child GameObject that acts as the highlight/outline.")]
    public GameObject outlineObject;

    private void OnEnable()
    {
        // Ensure the outline is always off when the object is enabled.
        if (outlineObject != null)
        {
            outlineObject.SetActive(false);
        }
    }
}

