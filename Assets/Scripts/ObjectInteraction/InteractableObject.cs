using UnityEngine;

/// <summary>
/// Attach this script to any GameObject you want to be interactable.
/// It holds the text for display and a reference to its highlight outline.
/// </summary>
public class InteractableObject : MonoBehaviour
{
    [Header("UI Display")]
    [Tooltip("The short text that appears when the player looks at the object.")]
    public string interactionPrompt;

    [Tooltip("The full text that is displayed when the player interacts with the object.")]
    [TextArea(3, 10)] // This makes the text field larger in the Inspector.
    public string noteText;

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

